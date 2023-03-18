using CheapChic.Data;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Configuration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CheapChic.Infrastructure.HostedServices;

public class WebhookHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public WebhookHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        
        var telegramBot = scope.ServiceProvider.GetRequiredService<ITelegramBot>();
        var managementBotOptions = scope.ServiceProvider.GetRequiredService<IOptions<ManagementBotOptions>>();
        
        await telegramBot.SetWebhook(managementBotOptions.Value.Token, cancellationToken);

        var tokens = await GetTelegramBotTokens(cancellationToken);

        foreach (var token in tokens)
        {
            await telegramBot.SetWebhook(token, cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        
        var telegramBot = scope.ServiceProvider.GetRequiredService<ITelegramBot>();
        var managementBotOptions = scope.ServiceProvider.GetRequiredService<IOptions<ManagementBotOptions>>();
        
        await telegramBot.DeleteWebhook(managementBotOptions.Value.Token, cancellationToken);

        var tokens = await GetTelegramBotTokens(cancellationToken);

        foreach (var token in tokens)
        {
            await telegramBot.DeleteWebhook(token, cancellationToken);
        }
    }

    private async Task<List<string>> GetTelegramBotTokens(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<CheapChicContext>();
        
        return await context.TelegramBots
            .AsNoTracking()
            .Where(x => !x.Disabled)
            .Select(x => x.Token)
            .ToListAsync(cancellationToken);
    }
}