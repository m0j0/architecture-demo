using ArchitectureDemo.States;
using DiscriminatedUnionGenerator;

namespace ArchitectureDemo.Results;

[DiscriminatedUnionCase(typeof(LockAcquired))]
[DiscriminatedUnionCase(typeof(AlreadyLocked))]
public sealed partial record LockResult : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        if (IsLockAcquired)
        {
            await AsLockAcquired.Lock.DisposeAsync();
        }
    }
}
