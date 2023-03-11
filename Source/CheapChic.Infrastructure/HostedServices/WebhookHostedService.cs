using CheapChic.Data;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Configuration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CheapChic.Infrastructure.HostedServices;

public class WebhookHostedService : IHostedService
{
    private readonly CheapChicContext _context;
    private readonly IOptions<ManagementBotOptions> _managementBotOptions;
    private readonly ITelegramBot _telegramBot;

    public WebhookHostedService(IOptions<ManagementBotOptions> managementBotOptions, CheapChicContext context,
        ITelegramBot telegramBot)
    {
        _managementBotOptions = managementBotOptions;
        _context = context;
        _telegramBot = telegramBot;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _telegramBot.SetWebhook(_managementBotOptions.Value.Token, cancellationToken);

        var tokens = await GetTelegramBotTokens(cancellationToken);

        foreach (var token in tokens)
        {
            await _telegramBot.SetWebhook(token, cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _telegramBot.DeleteWebhook(_managementBotOptions.Value.Token, cancellationToken);

        var tokens = await GetTelegramBotTokens(cancellationToken);

        foreach (var token in tokens)
        {
            await _telegramBot.DeleteWebhook(token, cancellationToken);
        }
    }

    private Task<List<string>> GetTelegramBotTokens(CancellationToken cancellationToken)
    {
        return _context.TelegramBots
            .AsNoTracking()
            .Where(x => !x.Disabled)
            .Select(x => x.Token)
            .ToListAsync(cancellationToken);
    }
}