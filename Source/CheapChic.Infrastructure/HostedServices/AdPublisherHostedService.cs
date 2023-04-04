using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.AdMessageBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheapChic.Infrastructure.HostedServices;

public class AdPublisherHostedService : IHostedService, IAsyncDisposable
{
    private const int Delay = 10;
    private readonly Task _completedTask = Task.CompletedTask;
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public AdPublisherHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask DisposeAsync()
    {
        if (_timer is IAsyncDisposable timer)
        {
            await timer.DisposeAsync();
        }

        _timer = null;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(TimerAction, null, TimeSpan.Zero, TimeSpan.FromSeconds(Delay));

        return _completedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return _completedTask;
    }

    private void TimerAction(object state)
    {
        Task.Run(async () => await PublishAds()).GetAwaiter().GetResult();
    }

    private async Task PublishAds()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CheapChicContext>();
        var telegramBot = scope.ServiceProvider.GetRequiredService<ITelegramBot>();
        var adBuilder = scope.ServiceProvider.GetRequiredService<IAdMessageBuilder>();

        var telegramBots = await context.TelegramBots
            .AsNoTracking()
            .Where(x => !x.Disabled)
            .Select(x => new { x.Id, x.Token, x.PublishEveryHours, x.Currency })
            .ToListAsync();

        foreach (var telegramBotData in telegramBots)
        {
            var date = DateTime.UtcNow.AddHours(-1 * telegramBotData.PublishEveryHours);
            var ad = await context.Ads
                .Where(x => !x.Disable)
                .Where(x => x.DateOfLastPublication == null || x.DateOfLastPublication <= date)
                .Where(x => x.BotId == telegramBotData.Id)
                .Include(x => x.User)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();

            if (ad is not null)
            {
                await PublishAd(context, telegramBot, adBuilder, ad, telegramBotData.Token, telegramBotData.Currency);
            }
        }
    }

    private static async Task PublishAd(CheapChicContext context, ITelegramBot telegramBot, IAdMessageBuilder adBuilder,
        AdEntity ad, string token, string currency)
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
            var request = adBuilder.BuildByEntity(ad, photos, channelChatId, ad.User.Username, currency);

            switch (request)
            {
                case SendMediaGroupRequest mediaGroupRequest:
                    await telegramBot.SendPhoto(token, mediaGroupRequest);
                    break;
                case SendTextMessageRequest textRequest:
                    await telegramBot.SendText(token, textRequest);
                    break;
            }
        }

        ad.DateOfLastPublication = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }
}