namespace CheapChic.Infrastructure.Bot.Requests;

public class SendMediaGroupRequest : SendRequest
{
    public string Text { get; set; }
    public Guid[] PhotoIds { get; set; }

    public SendMediaGroupRequest()
    {
    }

    private SendMediaGroupRequest(long chatId, string text, Guid[] photoIds) : base(chatId)
    {
        Text = text;
        PhotoIds = photoIds;
    }

    public static SendMediaGroupRequest Create(long chatId, string text, Guid[] photoIds)
    {
        return new SendMediaGroupRequest(chatId, text, photoIds);
    }
}