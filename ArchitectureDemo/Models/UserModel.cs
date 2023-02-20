namespace ArchitectureDemo.Models;

public sealed record UserModel(Guid Id, string Name, int FilesCount, Guid? ParentId, string? ParentName);
