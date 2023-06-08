using ArchitectureDemo.Models;
using ArchitectureDemo.Results;
using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Services;

public interface IUsersService
{
    Task<CreateUserResult> CreateUser(CreateUserModel model,
        CancellationToken cancellationToken);

    Task<UserModel?> GetUser(UserId id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<UserModel>> GetUsers(GetAllUsersFilter filter,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<UserWithChildrenModel>> GetUserTree(
        CancellationToken cancellationToken);
}
