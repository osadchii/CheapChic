using CheapChic.Data;
using CheapChic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.Services.TelegramBotService;

public class TelegramBotService : ITelegramBotService
{
    private readonly CheapChicContext _context;

    public TelegramBotService(CheapChicContext context)
    {
        _context = context;
    }

    public Task<TelegramBotEntity> GetTelegramBot(string token, CancellationToken cancellationToken)
    {
        return _context.TelegramBots
            .AsNoTracking()
            .Where(x => x.Disabled == false)
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }
}