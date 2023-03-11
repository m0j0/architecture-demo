namespace ArchitectureDemo.Infrastructure;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}
