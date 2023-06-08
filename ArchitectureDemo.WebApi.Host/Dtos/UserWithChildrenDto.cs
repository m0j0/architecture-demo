namespace ArchitectureDemo.WebApi.Host.Dtos;

public sealed class UserWithChildrenDto
{
    public UserWithChildrenDto(int id, string name, IReadOnlyList<UserWithChildrenDto> children)
    {
        Id = id;
        Name = name;
        Children = children;
    }

    public int Id { get; }

    public string Name { get; }

    public IReadOnlyList<UserWithChildrenDto> Children { get; }
}
