using DiscriminatedUnionAnalyzer;

namespace ArchitectureDemo.Results;

[DiscriminatedUnion]
public abstract class LockResult
{
    private LockResult()
    {
    }

    public sealed class Acquired : LockResult
    {
        public Acquired(IAsyncDisposable @lock)
        {
            Lock = @lock;
        }

        public IAsyncDisposable Lock { get; }
    }

    public sealed class AlreadyLocked : LockResult
    {
    }
}
