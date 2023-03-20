using CheapChic.Data;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Models;
using CheapChic.Infrastructure.Bot.Requests;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Text;

public class TextMessageHandler : ITextMessageHandler
{
    private readonly ITelegramBot _telegramBot;
    private readonly CheapChicContext _context;

    public TextMessageHandler(ITelegramBot telegramBot, CheapChicContext context)
    {
        _telegramBot = telegramBot;
        _context = context;
    }


    public async Task HandleTextMessage(string token, Telegram.Bot.Types.Message message, CancellationToken cancellationToken = default)
    {
        var chatId = message.Chat.Id;
        var text = message.Text!;
        
        await _telegramBot.SendText(token,
            SendTextMessageRequest.Create(chatId, $"Your said: {text}"), cancellationToken);

        var keyboard = ReplyKeyboardBuilder.Create()
            .AddRow()
            .AddButton("Мои боты")
            .AddButton("Добавить бота")
            .AddRow()
            .AddButton("Настройки")
            .AddButton("Объявления")
            .Build();

        await _telegramBot.SendReplyKeyboard(token,
            SendReplyKeyboardRequest.Create(chatId, $"Your said: {text}", keyboard),
            cancellationToken);
    }
}