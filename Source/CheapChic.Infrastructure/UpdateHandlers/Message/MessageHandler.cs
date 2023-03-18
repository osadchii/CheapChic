using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Requests;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message;

public class MessageHandler : IMessageHandler
{
    private readonly ITelegramBot _telegramBot;
    private readonly CheapChicContext _context;

    public MessageHandler(ITelegramBot telegramBot, CheapChicContext context)
    {
        _telegramBot = telegramBot;
        _context = context;
    }

    public async Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        await EnsureUserCreated(message, cancellationToken);
        
        await _telegramBot.SendText(token,
            SendTextMessageRequest.Create(message.Chat.Id, $"Your said: {message.Text}"), cancellationToken);
    }

    private async Task EnsureUserCreated(Telegram.Bot.Types.Message message, CancellationToken cancellationToken = default)
    {
        var chat = message.Chat;

        var chatId = chat.Id;
        var user = await _context.TelegramUsers
            .FirstOrDefaultAsync(x => x.ChatId == chatId, cancellationToken);

        if (user is null)
        {
            user = new TelegramUserEntity
            {
                ChatId = chatId,
                Name = chat.FirstName,
                Lastname = chat.LastName,
                Username = chat.Username,
                Disabled = false
            };
            
            await _context.AddAsync(user, cancellationToken);
        }
        else
        {
            user.Username = chat.Username;
            user.Lastname = chat.LastName;
            user.Username = chat.Username;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}