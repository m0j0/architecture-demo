namespace ArchitectureDemo.Models;

public sealed record UserWithChildrenModel(Guid Id, string Name, IReadOnlyList<UserWithChildrenModel> Children);
