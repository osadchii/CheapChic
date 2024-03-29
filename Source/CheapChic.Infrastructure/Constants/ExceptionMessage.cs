﻿namespace CheapChic.Infrastructure.Constants;

public static class ExceptionMessage
{
    public static string UserChannelNotFound(long chatId) => $"Channel or user with chat Id {chatId} not found";
    public static string UserNotFound(long chatId) => $"User with chat Id {chatId} not found";
    public static string BotNotFound => $"Bot not found";
}