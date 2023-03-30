using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management;

public interface IManagementStateActivator
{
    Task Activate(string token, TelegramUserEntity user, object stateData, CancellationToken cancellationToken = default);
}