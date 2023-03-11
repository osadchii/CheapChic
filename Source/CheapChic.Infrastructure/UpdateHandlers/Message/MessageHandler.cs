using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Requests;

namespace CheapChic.Infrastructure.UpdateHandlers.Message;

public class MessageHandler : IMessageHandler
{
    private readonly ITelegramBot _telegramBot;

    public MessageHandler(ITelegramBot telegramBot)
    {
        _telegramBot = telegramBot;
    }

    public Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        return _telegramBot.SendText(token,
            SendTextMessageRequest.Create(message.Chat.Id, $"Your said: {message.Text}"), cancellationToken);
        throw new NotImplementedException();
    }
}