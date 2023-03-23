using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States;

public interface IStateActivator
{
    Task Activate(string token, TelegramUserEntity user, object stateData, CancellationToken cancellationToken = default);
}