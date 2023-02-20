using ArchitectureDemo.States;
using DiscriminatedUnionGenerator;

namespace ArchitectureDemo.Results;

[DiscriminatedUnionCase(typeof(UserCreated))]
[DiscriminatedUnionCase(typeof(EmailAlreadyRegistered))]
public sealed partial record CreateUserResult;
