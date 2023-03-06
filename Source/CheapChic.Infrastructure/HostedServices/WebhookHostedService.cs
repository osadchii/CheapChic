using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.Constants;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace CheapChic.Infrastructure.HostedServices;

public class WebhookHostedService : IHostedService
{
    private readonly IOptions<ApplicationOptions> _applicationOptions;
    private readonly HttpClient _httpClient;
    private readonly IOptions<ManagementBotOptions> _managementBotOptions;

    public WebhookHostedService(IOptions<ManagementBotOptions> managementBotOptions,
        IOptions<ApplicationOptions> applicationOptions, HttpClient httpClient)
    {
        _managementBotOptions = managementBotOptions;
        _applicationOptions = applicationOptions;
        _httpClient = httpClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await SetWebhook(_managementBotOptions.Value.Token);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await DeleteWebhook(_managementBotOptions.Value.Token);
    }

    private async Task SetWebhook(string token)
    {
        var telegramBot = new TelegramBotClient(token, _httpClient);
        var webhookUrl =
            $"{_applicationOptions.Value.Host}" +
            $"{(_applicationOptions.Value.Host.EndsWith("/") ? string.Empty : "/")}" +
            $"{ControllerName.Telegram}/" +
            $"{token}";

        var webhookInfo = await telegramBot.GetWebhookInfoAsync();
        if (webhookInfo.Url != webhookUrl)
        {
            await telegramBot.SetWebhookAsync(webhookUrl);
        }
    }

    private async Task DeleteWebhook(string token)
    {
        var telegramBot = new TelegramBotClient(token, _httpClient);

        var webhookInfo = await telegramBot.GetWebhookInfoAsync();
        if (!string.IsNullOrEmpty(webhookInfo.Url))
        {
            await telegramBot.DeleteWebhookAsync();
        }
    }
}