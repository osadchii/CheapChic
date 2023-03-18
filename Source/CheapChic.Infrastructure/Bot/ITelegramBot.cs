using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Requests;

namespace CheapChic.Infrastructure.Bot;

public interface ITelegramBot
{
    Task SetWebhook(string token, CancellationToken cancellationToken = default);
    Task DeleteWebhook(string token, CancellationToken cancellationToken = default);
    Task<TelegramMessageEntity> SendText(string token, SendTextMessageRequest request, CancellationToken cancellationToken = default);
}