using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.Services.PhotoAdService;

public interface IPhotoAdService
{
    Task HandlePhoto(string token, Guid telegramBotId, TelegramUserEntity user, string fileId,
        CancellationToken cancellationToken = default);
}