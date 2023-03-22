using CheapChic.Data;
using CheapChic.Infrastructure.Services.UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.Services.UserService;

public class UserService : IUserService
{
    private readonly CheapChicContext _context;

    public UserService(CheapChicContext context)
    {
        _context = context;
    }

    public Task<UserState> GetUserState(Guid userId, Guid? botId, CancellationToken cancellationToken = default)
    {
        return _context.TelegramUserStates
            .Where(x => x.UserId == userId)
            .Where(x => x.TelegramBotId == botId)
            .Select(x => new UserState
            {
                State = x.State,
                Data = x.Data
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}