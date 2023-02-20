namespace ArchitectureDemo.Models;

public sealed record CreateUserModel(string Name, Guid? ParentId);
