using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer;

public interface IRetailerStateActivator
{
    Task Activate(TelegramBotEntity bot, TelegramUserEntity user, object stateData, CancellationToken cancellationToken = default);
}