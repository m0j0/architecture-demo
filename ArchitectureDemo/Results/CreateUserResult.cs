using ArchitectureDemo.States;
using DiscriminatedUnionGenerator;

namespace ArchitectureDemo.Results;

[DiscriminatedUnionCase(typeof(UserCreated))]
[DiscriminatedUnionCase(typeof(EmailAlreadyRegistered))]
[DiscriminatedUnionCase(typeof(ParentNotFound))]
public sealed partial record CreateUserResult;
