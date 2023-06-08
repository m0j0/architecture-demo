using ArchitectureDemo.Results;
using ArchitectureDemo.Services;
using ArchitectureDemo.Settings;
using Medallion.Threading.Postgres;
using Microsoft.Extensions.Options;

namespace ArchitectureDemo.DAL.Services;

internal class LockService : ILockService
{
    private readonly IOptionsMonitor<ConnectionStringSettings> _optionsMonitor;

    public LockService(IOptionsMonitor<ConnectionStringSettings> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }
    public async Task<LockResult> Acquire(int key1, int key2,
        CancellationToken cancellationToken)
    {
        var connectionString = _optionsMonitor.CurrentValue.DemoDb;
        var @lock = new PostgresDistributedLock(
            new PostgresAdvisoryLockKey(key1, key2),
            connectionString);

        var handle = await @lock.TryAcquireAsync(cancellationToken: cancellationToken);
        if (handle == null)
        {
            return new LockResult.AlreadyLocked();
        }

        return new LockResult.Acquired(handle);
    }
}
