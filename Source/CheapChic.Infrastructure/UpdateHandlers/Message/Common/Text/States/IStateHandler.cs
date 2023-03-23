using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text.States;

public interface IStateHandler
{
    Task Handle(string token, TelegramUserEntity user, string text, string stateData, CancellationToken cancellationToken);
}