using ArchitectureDemo.DAL.Entities;
using ArchitectureDemo.Models;
using ArchitectureDemo.Results;
using ArchitectureDemo.Services;
using ArchitectureDemo.States;
using ArchitectureDemo.ValueObjects;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ArchitectureDemo.DAL.Services;

internal class UsersService : IUsersService
{
    private record UserProjection(Guid Id, string Name, Guid? ParentId);

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
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                ParentId = model.ParentId
            };
            _demoContext.Users.Add(user);
            await _demoContext.SaveChangesAsync(cancellationToken);
            return new UserCreated(new UserId(user.Id));
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException
                                          {
                                              SqlState: PostgresErrorCodes.UniqueViolation,
                                              ConstraintName: User.EmailUniqueIndexName
                                          })
        {
            return new EmailAlreadyRegistered();
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException
                                          {
                                              SqlState: PostgresErrorCodes.ForeignKeyViolation,
                                              ConstraintName: User.ParentIdForeignKeyName
                                          })
        {
            return new ParentNotFound();
        }
    }

    public async Task<UserModel?> GetUser(UserId id, CancellationToken cancellationToken)
    {
        return await _demoContext
            .Users
            .Where(u => u.Id == id.Value)
            // TODO вынести в общее создание модели на уровне класса
            .Select(u => new UserModel(u.Id, u.Name, u.Files.Count, u.Parent!.Id, u.Parent.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<UserModel>> GetUsers(CancellationToken cancellationToken)
    {
        return await _demoContext
            .Users
            .Select(u => new UserModel(u.Id, u.Name, u.Files.Count, u.Parent!.Id, u.Parent.Name))
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
            .Select(rootUser => new UserWithChildrenModel(rootUser.Id, rootUser.Name, GetTree(users, rootUser).ToArray()))
            .ToArray();

        return roots;

        static IEnumerable<UserWithChildrenModel> GetTree(
            IReadOnlyList<UserProjection> allUsers,
            UserProjection parent)
        {
            foreach (var user in allUsers.Where(u => u.ParentId == parent.Id))
            {
                yield return new UserWithChildrenModel(user.Id, user.Name, GetTree(allUsers, user).ToArray());
            }
        }
    }
}
