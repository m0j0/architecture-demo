using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchitectureDemo.Models;
using ArchitectureDemo.Results;
using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Services;

public interface IUsersService
{
    Task<CreateUserResult> CreateUser(CreateUserModel model, CancellationToken cancellationToken);

    Task<UserModel?> GetUser(UserId id, CancellationToken cancellationToken);

    Task<IReadOnlyList<UserModel>> GetUsers(CancellationToken cancellationToken);

    Task<IReadOnlyList<UserWithChildrenModel>> GetUserTree(CancellationToken cancellationToken);
}
