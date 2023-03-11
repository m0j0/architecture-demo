using ArchitectureDemo.Results;

namespace ArchitectureDemo.Services;

public interface ILockService
{
    Task<LockResult> Acquire(string lockName, CancellationToken cancellationToken);
}
