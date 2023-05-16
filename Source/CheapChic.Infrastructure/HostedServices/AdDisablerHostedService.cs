using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.AdMessageBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheapChic.Infrastructure.HostedServices;

public class AdDisablerHostedService : IHostedService, IAsyncDisposable
{
    private const int Delay = 10;
    private readonly Task _completedTask = Task.CompletedTask;
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public AdDisablerHostedService(IServiceProvider serviceProvider)
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

        var telegramBots = await context.TelegramBots
            .AsNoTracking()
            .Where(x => !x.Disabled)
            .Select(x => new { x.Id, x.Token, x.PublishForDays })
            .ToListAsync();

        foreach (var telegramBotData in telegramBots)
        {
            var date = DateTime.UtcNow.AddDays(-1 * telegramBotData.PublishForDays);
            var ads = await context.Ads
                .Where(x => !x.Disable)
                .Where(x => x.Date <= date)
                .Where(x => x.BotId == telegramBotData.Id)
                .Include(x => x.User)
                .ToListAsync();

            foreach (var ad in ads)
            {
                ad.Disable = true;
            
                var requestMessage = SendTextMessageRequest.Create(ad.User.ChatId, MessageText.Retailer.Ad.AdHasBeenDisabled(ad.Name));
                await telegramBot.SendText(telegramBotData.Token, requestMessage);
            }

            await context.SaveChangesAsync();
        }
    }
}