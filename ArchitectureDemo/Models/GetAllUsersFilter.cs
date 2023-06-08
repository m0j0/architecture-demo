namespace ArchitectureDemo.Models;

public sealed record GetAllUsersFilter(string? Name, DateTimeOffset? CreatedAfter);
