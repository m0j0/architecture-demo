namespace ArchitectureDemo.WebApi.Host.Dtos;

public sealed class GetAllUsersFilterDto
{
    public string? Name { get; set; }

    public DateTimeOffset? CreatedAfter { get; set; }
}
