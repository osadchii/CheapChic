using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Extensions;
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
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Where(x => x.BotId == botId)
            .Select(x => new UserState
            {
                State = x.State,
                Data = x.Data
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SetUserState(Guid userId, Guid? botId, State state, object data,
        CancellationToken cancellationToken = default)
    {
        var userState = await _context.TelegramUserStates
            .Where(x => x.UserId == userId)
            .Where(x => x.BotId == botId)
            .FirstOrDefaultAsync(cancellationToken);

        if (userState is null)
        {
            userState = new TelegramUserStateEntity
            {
                UserId = userId,
                BotId = botId,
                State = state,
                Data = data.ToJson()
            };

            await _context.AddAsync(userState, cancellationToken);
        }
        else
        {
            userState.State = state;
            userState.Data = data.ToJson();
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}