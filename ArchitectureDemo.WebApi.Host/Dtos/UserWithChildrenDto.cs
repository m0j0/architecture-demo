namespace ArchitectureDemo.WebApi.Host.Dtos;

public sealed class UserWithChildrenDto
{
    public UserWithChildrenDto(Guid id, string name, IReadOnlyList<UserWithChildrenDto> children)
    {
        Id = id;
        Name = name;
        Children = children;
    }

    public Guid Id { get; }

    public string Name { get; }

    public IReadOnlyList<UserWithChildrenDto> Children { get; }
}

public static class UserWithChildrenModelMapper
{
    public static UserWithChildrenDto ToDto(this Models.UserWithChildrenModel user)
    {
        return new UserWithChildrenDto(user.Id, user.Name, user.Children.Select(c => c.ToDto()).ToArray());
    }
}
