using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Models;

public sealed record UserWithChildrenModel(UserId Id, string Name, IReadOnlyList<UserWithChildrenModel> Children);
