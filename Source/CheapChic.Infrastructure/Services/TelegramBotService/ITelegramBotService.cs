using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.Services.TelegramBotService;

public interface ITelegramBotService
{
    Task<TelegramBotEntity> GetTelegramBot(string token, CancellationToken cancellationToken);
}