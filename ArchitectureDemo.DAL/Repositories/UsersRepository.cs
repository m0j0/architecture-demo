using ArchitectureDemo.DAL.Entities;
using ArchitectureDemo.Repositories;
using ArchitectureDemo.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureDemo.DAL.Repositories;

internal class UsersRepository : IUsersRepository
{
    private readonly DemoContext _demoContext;

    public UsersRepository(DemoContext demoContext)
    {
        _demoContext = demoContext;
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
            CreationDate = DateTimeOffset.UtcNow
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
