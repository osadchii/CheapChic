using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text;

public class ManagementTextHandler : IManagementTextMessageHandler
{
    private readonly ITelegramBot _telegramBot;

    public ManagementTextHandler(ITelegramBot telegramBot)
    {
        _telegramBot = telegramBot;
    }

    public async Task HandleTextMessage(string token, Telegram.Bot.Types.Message message, CancellationToken cancellationToken = default)
    {
        var chatId = message.Chat.Id;
        var text = message.Text!;
        
        await _telegramBot.SendText(token,
            SendTextMessageRequest.Create(chatId, $"Your said: {text}"), cancellationToken);

        await _telegramBot.SendReplyKeyboard(token,
            SendReplyKeyboardRequest.Create(chatId, $"Your said: {text}", ConstantMenu.Management.ManagementMainMenu),
            cancellationToken);
    }
}