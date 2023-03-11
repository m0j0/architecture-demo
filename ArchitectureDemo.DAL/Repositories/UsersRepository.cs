using ArchitectureDemo.DAL.Entities;
using ArchitectureDemo.Infrastructure;
using ArchitectureDemo.Repositories;
using ArchitectureDemo.Results;
using ArchitectureDemo.States;
using ArchitectureDemo.ValueObjects;
using Medallion.Threading.Postgres;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureDemo.DAL.Repositories;

internal class UsersRepository : IUsersRepository
{
    private readonly DemoContext _demoContext;
    private readonly ISystemClock _systemClock;

    public UsersRepository(DemoContext demoContext, ISystemClock systemClock)
    {
        _demoContext = demoContext;
        _systemClock = systemClock;
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
            Name = fileName,
            CreationDate = _systemClock.UtcNow.UtcDateTime
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
