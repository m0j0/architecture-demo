using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Models;

public sealed record UserModel(UserId Id, string Name, int FilesCount, UserId? ParentId, string? ParentName);
