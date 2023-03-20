using CheapChic.Infrastructure.Bot.Models;

namespace CheapChic.Infrastructure.Bot.Requests;

public class SendReplyKeyboardRequest : SendRequest
{
    public string Text { get; set; }
    public ReplyKeyboardModel Keyboard { get; set; }

    private SendReplyKeyboardRequest(long chatId, string text, ReplyKeyboardModel keyboard) : base(chatId)
    {
        Text = text;
        Keyboard = keyboard;
    }

    public static SendReplyKeyboardRequest Create(long chatId, string text, ReplyKeyboardModel keyboard)
    {
        return new SendReplyKeyboardRequest(chatId, text, keyboard);
    }
}