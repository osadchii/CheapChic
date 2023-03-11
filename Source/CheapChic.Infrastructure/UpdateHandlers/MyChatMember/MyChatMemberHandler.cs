using Telegram.Bot.Types;

namespace CheapChic.Infrastructure.UpdateHandlers.MyChatMember;

public class MyChatMemberHandler : IMyChatMemberHandler
{
    public Task HandleMyChatMember(string token, ChatMemberUpdated myChatMember,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}