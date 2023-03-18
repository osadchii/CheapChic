namespace CheapChic.Infrastructure.Constants;

public static class ExceptionMessage
{
    public static string UserChannelNotFound(long chatId) => $"Channel or user with chat Id {chatId} not found";
    public static string BotNotFound(string token) => $"Bot with token {token} not found";
}