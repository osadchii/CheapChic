namespace CheapChic.Infrastructure.UpdateHandlers.CallbackQuery;

public interface ICallbackQueryHandler
{
    Task HandleCallbackQuery(string token, Telegram.Bot.Types.CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default);
}