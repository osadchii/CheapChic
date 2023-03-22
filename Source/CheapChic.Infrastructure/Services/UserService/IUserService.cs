using CheapChic.Infrastructure.Services.UserService.Models;

namespace CheapChic.Infrastructure.Services.UserService;

public interface IUserService
{
    Task<UserState> GetUserState(Guid userId, Guid? botId, CancellationToken cancellationToken = default);

    Task<UserState> GetUserState(Guid userId, CancellationToken cancellationToken = default) =>
        GetUserState(userId, null, cancellationToken);
}