namespace ArchitectureDemo.Models;

public sealed record CreateUserModel(string Name, string Email, Guid? ParentId);
