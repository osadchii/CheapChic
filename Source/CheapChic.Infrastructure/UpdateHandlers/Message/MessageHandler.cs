using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Document;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Photo;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace CheapChic.Infrastructure.UpdateHandlers.Message;

public class MessageHandler : IMessageHandler
{
    private readonly CheapChicContext _context;
    private readonly ITextMessageHandler _textMessageHandler;
    private readonly IDocumentMessageHandler _documentMessageHandler;
    private readonly IPhotoMessageHandler _photoMessageHandler;

    public MessageHandler(CheapChicContext context, ITextMessageHandler textMessageHandler,
        IDocumentMessageHandler documentMessageHandler, IPhotoMessageHandler photoMessageHandler)
    {
        _context = context;
        _textMessageHandler = textMessageHandler;
        _documentMessageHandler = documentMessageHandler;
        _photoMessageHandler = photoMessageHandler;
    }

    public async Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        await EnsureUserCreated(message, cancellationToken);

        ITypeMessageHandler messageHandler = message.Type switch
        {
            MessageType.Text => _textMessageHandler,
            MessageType.Photo => _photoMessageHandler,
            MessageType.Document => _documentMessageHandler,
            _ => null
        };

        if (messageHandler is not null)
        {
            await messageHandler.HandleMessage(token, message, cancellationToken);
        }
    }

    private async Task EnsureUserCreated(Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
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