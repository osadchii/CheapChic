using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;

namespace CheapChic.Infrastructure.UpdateHandlers.Message;

public class MessageHandler : IMessageHandler
{
    private readonly CheapChicContext _context;
    private readonly ITextMessageHandler _textMessageHandler;

    public MessageHandler(CheapChicContext context, ITextMessageHandler textMessageHandler)
    {
        _context = context;
        _textMessageHandler = textMessageHandler;
    }

    public async Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        await EnsureUserCreated(message, cancellationToken);

        switch (message.Type)
        {
            case MessageType.Text:
                await _textMessageHandler.HandleTextMessage(token, message, cancellationToken);
                break;
            case MessageType.Unknown:
            case MessageType.Photo:
            case MessageType.Audio:
            case MessageType.Video:
            case MessageType.Voice:
            case MessageType.Document:
            case MessageType.Sticker:
            case MessageType.Location:
            case MessageType.Contact:
            case MessageType.Venue:
            case MessageType.Game:
            case MessageType.VideoNote:
            case MessageType.Invoice:
            case MessageType.SuccessfulPayment:
            case MessageType.WebsiteConnected:
            case MessageType.ChatMembersAdded:
            case MessageType.ChatMemberLeft:
            case MessageType.ChatTitleChanged:
            case MessageType.ChatPhotoChanged:
            case MessageType.MessagePinned:
            case MessageType.ChatPhotoDeleted:
            case MessageType.GroupCreated:
            case MessageType.SupergroupCreated:
            case MessageType.ChannelCreated:
            case MessageType.MigratedToSupergroup:
            case MessageType.MigratedFromGroup:
            case MessageType.Poll:
            case MessageType.Dice:
            case MessageType.MessageAutoDeleteTimerChanged:
            case MessageType.ProximityAlertTriggered:
            case MessageType.WebAppData:
            case MessageType.VideoChatScheduled:
            case MessageType.VideoChatStarted:
            case MessageType.VideoChatEnded:
            case MessageType.VideoChatParticipantsInvited:
            default:
                break;
        }
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