namespace CheapChic.Infrastructure.UpdateHandlers.Message;

public interface IMessageHandler
{
    Task HandleMessage(string token, Telegram.Bot.Types.Message message, CancellationToken cancellationToken = default);
}