namespace ArchitectureDemo.Models;

public sealed class GetAllUsersFilter
{
    public GetAllUsersFilter(string? name, DateTimeOffset? createdAfter)
    {
        Name = name;
        CreatedAfter = createdAfter;
    }

    public string? Name { get; }

    public DateTimeOffset? CreatedAfter { get; }
}
