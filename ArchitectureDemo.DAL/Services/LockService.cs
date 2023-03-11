using ArchitectureDemo.Results;
using ArchitectureDemo.Services;
using ArchitectureDemo.States;
using Medallion.Threading.Postgres;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureDemo.DAL.Services;

internal class LockService : ILockService
{
    private readonly DemoContext _demoContext;

    public LockService(DemoContext demoContext)
    {
        _demoContext = demoContext;
    }
    public async Task<LockResult> Acquire(string lockName, CancellationToken cancellationToken)
    {
        var connectionString = _demoContext.Database.GetConnectionString() ??
                               throw new InvalidOperationException();

        var @lock = new PostgresDistributedLock(new PostgresAdvisoryLockKey(lockName, allowHashing: true), connectionString);

        var handle = await @lock.TryAcquireAsync(cancellationToken: cancellationToken);
        if (handle == null)
        {
            return new AlreadyLocked();
        }

        return new LockAcquired(handle);
    }
}
