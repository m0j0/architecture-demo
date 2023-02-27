using ArchitectureDemo.Models;
using ArchitectureDemo.Results;
using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Repositories;

public interface IUsersRepository
{
    Task<LockResult> LockUser(UserId userId, CancellationToken cancellationToken);

    Task<bool> DoesUserExist(UserId userId, CancellationToken cancellationToken);
        
    Task<FileId> AddFileToUser(UserId userId, string fileName, CancellationToken cancellationToken);

    Task<int> GetUserFilesCount(UserId userId, CancellationToken cancellationToken);

    Task<string?> GetFileName(FileId fileId, CancellationToken cancellationToken);
}
