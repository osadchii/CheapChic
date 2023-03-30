using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer;

public interface IRetailerStateHandler
{
    Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData, CancellationToken cancellationToken);
}