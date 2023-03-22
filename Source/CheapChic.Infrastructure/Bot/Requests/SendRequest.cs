namespace CheapChic.Infrastructure.Bot.Requests;

public abstract class SendRequest
{
    protected SendRequest(long chatId)
    {
        ChatId = chatId;
    }

    protected SendRequest()
    {
        
    }

    public long ChatId { get; }
}