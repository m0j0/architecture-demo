using System.Linq.Expressions;
using ArchitectureDemo.DAL.Entities;
using ArchitectureDemo.Models;
using ArchitectureDemo.Results;
using ArchitectureDemo.Services;
using ArchitectureDemo.ValueObjects;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NeinLinq;
using Npgsql;

namespace ArchitectureDemo.DAL.Services;

internal class UsersService : IUsersService
{
    private record UserProjection(int Id, string Name, int? ParentId);
    
    private static readonly Expression<Func<User, UserModel>> UserDbToModelExpression =
        u => new UserModel(new UserId(u.Id),
            u.Name,
            u.Files.Count,
            u.Parent == null ? null : new UserId(u.Parent!.Id),
            u.Parent == null ? null : u.Parent.Name);

    private readonly DemoContext _demoContext;

    public UsersService(DemoContext demoContext)
    {
        _demoContext = demoContext;
    }

    public async Task<CreateUserResult> CreateUser(CreateUserModel model,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                ParentId = model.ParentId?.Value,
                CreationDate = DateTimeOffset.UtcNow
            };
            _demoContext.Users.Add(user);
            await _demoContext.SaveChangesAsync(cancellationToken);
            return new CreateUserResult.Created(new UserId(user.Id));
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException
                                          {
                                              SqlState: PostgresErrorCodes.UniqueViolation,
                                              ConstraintName: User.EmailUniqueIndexName
                                          })
        {
            return new CreateUserResult.EmailAlreadyRegistered();
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException
                                          {
                                              SqlState: PostgresErrorCodes.ForeignKeyViolation,
                                              ConstraintName: User.ParentIdForeignKeyName
                                          })
        {
            return new CreateUserResult.ParentNotFound();
        }
    }

    public async Task<UserModel?> GetUser(UserId id, CancellationToken cancellationToken)
    {
        return await _demoContext
            .Users
            .Where(u => u.Id == id.Value)
            .Select(UserDbToModelExpression)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserModel>> GetUsers(GetAllUsersFilter filter,
        CancellationToken cancellationToken)
    {
        return await _demoContext
            .Users
            .ToInjectable()
            .Where(u => u.FilterByName(filter.Name) &&
                        u.FilterByCreationDate(filter.CreatedAfter))
            .Select(UserDbToModelExpression)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserWithChildrenModel>> GetUserTree(CancellationToken cancellationToken)
    {
        const string sql = @"select id as Id, name as Name, parent_id as ParentId from users";

        var users = (await _demoContext
                .Database
                .GetDbConnection()
                .QueryAsync<UserProjection>(new CommandDefinition(sql, cancellationToken: cancellationToken)))
            .ToArray();

        var roots = users
            .Where(u => u.ParentId is null)
            .Select(rootUser => new UserWithChildrenModel(new UserId(rootUser.Id) ,rootUser.Name, GetTree(users, rootUser).ToArray()))
            .ToArray();

        return roots;

        static IEnumerable<UserWithChildrenModel> GetTree(
            IReadOnlyList<UserProjection> allUsers,
            UserProjection parent)
        {
            foreach (var user in allUsers.Where(u => u.ParentId.HasValue && u.ParentId.Value == parent.Id))
            {
                yield return new UserWithChildrenModel(new UserId(user.Id), user.Name, GetTree(allUsers, user).ToArray());
            }
        }
    }
}
