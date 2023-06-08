namespace ArchitectureDemo.WebApi.Host.Dtos;

public class UserDto
{
    public UserDto(int id, string name, int filesCount, int? parentId, string? parentName)
    {
        Id = id;
        Name = name;
        FilesCount = filesCount;
        ParentId = parentId;
        ParentName = parentName;
    }

    public int Id { get; }

    public string Name { get; }

    public int FilesCount { get; }

    public int? ParentId { get; }

    public string? ParentName { get; }
}
