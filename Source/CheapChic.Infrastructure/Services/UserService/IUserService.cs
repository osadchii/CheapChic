using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Services.UserService.Models;

namespace CheapChic.Infrastructure.Services.UserService;

public interface IUserService
{
    Task<UserState> GetUserState(Guid userId, Guid? botId, CancellationToken cancellationToken = default);

    Task<UserState> GetUserState(Guid userId, CancellationToken cancellationToken = default) =>
        GetUserState(userId, null, cancellationToken);

    Task SetUserState(Guid userId, Guid? botId, State state, object data,
        CancellationToken cancellationToken = default);

    Task SetUserState(Guid userId, State state, object data,
        CancellationToken cancellationToken = default) => SetUserState(userId, null, state, data, cancellationToken);

    Task<TelegramUserEntity> GetUser(long chatId, CancellationToken cancellationToken = default);
}