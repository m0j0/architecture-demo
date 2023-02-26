namespace ArchitectureDemo.WebApi.Host.Dtos;

public class UserDto
{
    public UserDto(Guid id, string name, int filesCount, Guid? parentId, string? parentName)
    {
        Id = id;
        Name = name;
        FilesCount = filesCount;
        ParentId = parentId;
        ParentName = parentName;
    }

    public Guid Id { get; }

    public string Name { get; }

    public int FilesCount { get; }

    public Guid? ParentId { get; }

    public string? ParentName { get; }
}

public static class UserModelMapper
{
    public static UserDto ToDto(this Models.UserModel user)
    {
        return new UserDto(user.Id, user.Name, user.FilesCount, user.ParentId, user.ParentName);
    }
}
