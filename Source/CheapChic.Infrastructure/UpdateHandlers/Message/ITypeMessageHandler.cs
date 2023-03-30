namespace CheapChic.Infrastructure.UpdateHandlers.Message;

public interface ITypeMessageHandler
{
    Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default);
}