using Telegram.Bot.Types;

namespace CheapChic.Infrastructure.UpdateHandlers.MyChatMember;

public interface IMyChatMemberHandler
{
    Task HandleMyChatMember(string token, ChatMemberUpdated myChatMember);
}