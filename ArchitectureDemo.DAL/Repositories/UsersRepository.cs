using ArchitectureDemo.DAL.Entities;
using ArchitectureDemo.Repositories;
using ArchitectureDemo.Results;
using ArchitectureDemo.States;
using ArchitectureDemo.ValueObjects;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ArchitectureDemo.DAL.Repositories;

internal class UsersRepository : IUsersRepository
{
    private class Lock : IAsyncDisposable
    {
        private readonly IDbContextTransaction _transaction;
        private readonly CancellationToken _cancellationToken;

        public Lock(IDbContextTransaction transaction, CancellationToken cancellationToken)
        {
            _transaction = transaction;
            _cancellationToken = cancellationToken;
        }

        public async ValueTask DisposeAsync()
        {
            await _transaction.CommitAsync(_cancellationToken);
            await _transaction.DisposeAsync();
        }
    }

    private readonly DemoContext _demoContext;

    public UsersRepository(DemoContext demoContext)
    {
        _demoContext = demoContext;
    }

    public async Task<LockResult> LockUser(UserId userId, CancellationToken cancellationToken)
    {
        const string sql = "select id from users where id = @userId for update;";

        var transaction = await _demoContext.Database.BeginTransactionAsync(cancellationToken);

        var connection = _demoContext.Database.GetDbConnection();

        var loadedUserId = await connection.QueryFirstOrDefaultAsync<int?>(
            new CommandDefinition(sql, new { userId = userId.Value }, cancellationToken: cancellationToken)
        );

        if (loadedUserId.HasValue && loadedUserId.Value == userId.Value)
        {
            return new LockAcquired(new Lock(transaction, cancellationToken));
        }

        await transaction.RollbackAsync(cancellationToken);
        return new UserNotFound();
    }

    public async Task<bool> DoesUserExist(UserId userId, CancellationToken cancellationToken)
    {
        return await _demoContext
            .Users
            .Where(u => u.Id == userId.Value)
            .AnyAsync(cancellationToken);
    }

    public async Task<FileId> AddFileToUser(UserId userId, string fileName, CancellationToken cancellationToken)
    {
        var newEntity = new UserFile
        {
            UserId = userId.Value,
            Name = fileName
        };
        _demoContext.UserFiles.Add(newEntity);
        await _demoContext.SaveChangesAsync(cancellationToken);

        return new FileId(newEntity.Id);
    }

    public async Task<int> GetUserFilesCount(UserId userId, CancellationToken cancellationToken)
    {
        return await _demoContext
            .UserFiles
            .Where(uf => uf.UserId == userId.Value)
            .CountAsync(cancellationToken);
    }

    public async Task<string?> GetFileName(FileId fileId, CancellationToken cancellationToken)
    {
        return await _demoContext
            .UserFiles
            .Where(uf => uf.Id == fileId.Value)
            .Select(uf => uf.Name)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
