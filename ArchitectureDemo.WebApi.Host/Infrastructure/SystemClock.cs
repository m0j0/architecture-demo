using ArchitectureDemo.Infrastructure;

namespace ArchitectureDemo.WebApi.Host.Infrastructure;

public class SystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.Now;
}
