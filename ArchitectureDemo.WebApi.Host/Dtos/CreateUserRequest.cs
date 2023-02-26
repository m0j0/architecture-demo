namespace ArchitectureDemo.WebApi.Host.Dtos;

public class CreateUserRequest
{
    public string Name { get; set; } = null!;

    public Guid? ParentId { get; set; }
}
