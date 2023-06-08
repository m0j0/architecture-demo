using ArchitectureDemo.Results;

namespace ArchitectureDemo.Services;

public interface ILockService
{
    Task<LockResult> Acquire(int key1, int key2, CancellationToken cancellationToken);
}

public static class LockConstants
{
    public const int User = 1;
}
