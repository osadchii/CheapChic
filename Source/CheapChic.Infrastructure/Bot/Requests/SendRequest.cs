namespace CheapChic.Infrastructure.Bot.Requests;

public abstract class SendRequest
{
    protected SendRequest(long chatId)
    {
        ChatId = chatId;
    }

    public long ChatId { get; }
}