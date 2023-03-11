namespace CheapChic.Infrastructure.UpdateHandlers.CallbackQuery;

public class CallbackQueryHandler : ICallbackQueryHandler
{
    public Task HandleCallbackQuery(string token, Telegram.Bot.Types.CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}