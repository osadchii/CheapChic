using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management;

public interface IManagementStateHandler
{
    Task Handle(string token, TelegramUserEntity user, string text, string stateData, CancellationToken cancellationToken);
}