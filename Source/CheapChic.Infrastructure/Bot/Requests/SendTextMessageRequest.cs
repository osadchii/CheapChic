namespace CheapChic.Infrastructure.Bot.Requests;

public class SendTextMessageRequest : SendRequest
{
    private SendTextMessageRequest(long chatId, string text) : base(chatId)
    {
        Text = text;
    }

    public SendTextMessageRequest()
    {
        
    }

    public string Text { get; }

    public static SendTextMessageRequest Create(long chatId, string text)
    {
        return new SendTextMessageRequest(chatId, text);
    }
}