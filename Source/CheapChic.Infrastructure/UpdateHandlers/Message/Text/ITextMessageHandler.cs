namespace CheapChic.Infrastructure.UpdateHandlers.Message.Text;

public interface ITextMessageHandler
{
    Task HandleTextMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default);
}