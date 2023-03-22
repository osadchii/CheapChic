namespace CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text;

public interface ITextMessageHandler
{
    Task HandleTextMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default);
}