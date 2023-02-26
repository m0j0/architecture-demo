using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Models;

public sealed record CreateUserModel(string Name, string Email, UserId? ParentId);
