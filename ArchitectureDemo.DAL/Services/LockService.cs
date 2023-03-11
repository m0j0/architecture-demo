using ArchitectureDemo.Results;
using ArchitectureDemo.Services;
using ArchitectureDemo.Settings;
using ArchitectureDemo.States;
using Medallion.Threading.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ArchitectureDemo.DAL.Services;

internal class LockService : ILockService
{
    private readonly IOptionsMonitor<ConnectionStringSettings> _optionsMonitor;

    public LockService(IOptionsMonitor<ConnectionStringSettings> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }
    public async Task<LockResult> Acquire(string lockName, CancellationToken cancellationToken)
    {
        var connectionString = _optionsMonitor.CurrentValue.DemoDb;
        var @lock = new PostgresDistributedLock(new PostgresAdvisoryLockKey(lockName, allowHashing: true), connectionString);

        var handle = await @lock.TryAcquireAsync(cancellationToken: cancellationToken);
        if (handle == null)
        {
            return new AlreadyLocked();
        }

        return new LockAcquired(handle);
    }
}
