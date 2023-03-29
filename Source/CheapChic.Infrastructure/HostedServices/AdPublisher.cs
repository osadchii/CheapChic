using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.AdMessageBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheapChic.Infrastructure.HostedServices;

public class AdPublisher : IHostedService, IAsyncDisposable
{
    private readonly Task _completedTask = Task.CompletedTask;
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    private const int PublishEveryMinutes = 1440;
    private const int Delay = 30;

    public AdPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(TimerAction, null, TimeSpan.Zero, TimeSpan.FromSeconds(Delay));

        return _completedTask;
    }

    private void TimerAction(object state)
    {
        Task.Run(async () => await PublishAds());
    }

    private async Task PublishAds()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CheapChicContext>();
        var telegramBot = scope.ServiceProvider.GetRequiredService<ITelegramBot>();
        var adBuilder = scope.ServiceProvider.GetRequiredService<IAdMessageBuilder>();

        var date = DateTime.UtcNow.AddMinutes(-1 * PublishEveryMinutes);
        var ads = await context.Ads
            .Where(x => !x.Disable)
            .Where(x => x.DateOfLastPublication == null || x.DateOfLastPublication < date)
            .Include(x => x.Bot)
            .Include(x => x.User)
            .ToListAsync();

        foreach (var ad in ads)
        {
            await PublishAd(context, telegramBot, adBuilder, ad);
            await Task.Delay(TimeSpan.FromSeconds(15));
        }
    }

    private static async Task PublishAd(CheapChicContext context, ITelegramBot telegramBot, IAdMessageBuilder adBuilder,
        AdEntity ad)
    {
        var photos = await context.AdPhotos
            .AsNoTracking()
            .Where(x => x.AdId == ad.Id)
            .ToListAsync();

        var channels = await context.TelegramBotChannelMappings
            .AsNoTracking()
            .Include(x => x.Channel)
            .Where(x => x.BotId == ad.BotId)
            .Select(x => x.Channel.ChatId)
            .ToListAsync();

        foreach (var channelChatId in channels)
        {
            var request = adBuilder.BuildByEntity(ad, photos, channelChatId, ad.User.Username);

            switch (request)
            {
                case SendMediaGroupRequest mediaGroupRequest:
                    await telegramBot.SendPhoto(ad.Bot.Token, mediaGroupRequest);
                    break;
                case SendTextMessageRequest textRequest:
                    await telegramBot.SendText(ad.Bot.Token, textRequest);
                    break;
            }
        }
        
        ad.DateOfLastPublication = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return _completedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_timer is IAsyncDisposable timer)
        {
            await timer.DisposeAsync();
        }

        _timer = null;
    }
}