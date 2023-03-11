using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.Constants;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CheapChic.Infrastructure.Bot;

public class TelegramBot : ITelegramBot
{
    private readonly IOptions<ApplicationOptions> _applicationOptions;
    private readonly HttpClient _httpClient;

    public TelegramBot(HttpClient httpClient, IOptions<ApplicationOptions> applicationOptions)
    {
        _httpClient = httpClient;
        _applicationOptions = applicationOptions;
    }

    public async Task SetWebhook(string token, CancellationToken cancellationToken = default)
    {
        var host = _applicationOptions.Value.Host;
        var client = GetClient(token);

        var webhookUrl =
            $"{host}" +
            $"{(host.EndsWith("/") ? string.Empty : "/")}" +
            $"{ControllerName.Telegram}/" +
            $"{token}";

        var webhookInfo = await client.GetWebhookInfoAsync(cancellationToken: cancellationToken);
        if (webhookInfo.Url != webhookUrl)
        {
            await client.SetWebhookAsync(webhookUrl, cancellationToken: cancellationToken);
        }
    }

    public async Task DeleteWebhook(string token, CancellationToken cancellationToken = default)
    {
        var client = GetClient(token);

        var webhookInfo = await client.GetWebhookInfoAsync(cancellationToken: cancellationToken);
        if (!string.IsNullOrEmpty(webhookInfo.Url))
        {
            await client.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }

    public Task SendText(string token, SendTextMessageRequest request, CancellationToken cancellationToken = default)
    {
        var chatId = request.ChatId;
        var text = request.Text;

        var client = GetClient(token);

        return client.SendTextMessageAsync(chatId, text, ParseMode.Html, cancellationToken: cancellationToken);
    }

    private TelegramBotClient GetClient(string token) => new(token, _httpClient);
}