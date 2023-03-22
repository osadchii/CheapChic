using CheapChic.Data;
using CheapChic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text;

public class RetailerTextMessageHandler : IRetailerTextMessageHandler
{
    private readonly CheapChicContext _context;

    public RetailerTextMessageHandler(CheapChicContext context)
    {
        _context = context;
    }

    public async Task HandleTextMessage(string token, Telegram.Bot.Types.Message message, CancellationToken cancellationToken = default)
    {
        var telegramBot = await GetTelegramBot(token, cancellationToken);

        if (telegramBot is null)
        {
            return;
        }
        
        throw new NotImplementedException();
    }

    private Task<TelegramBotEntity> GetTelegramBot(string token, CancellationToken cancellationToken)
    {
        return _context.TelegramBots
            .AsNoTracking()
            .Where(x => x.Disabled == false)
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }
}