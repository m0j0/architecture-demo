using ArchitectureDemo.ValueObjects;
using DiscriminatedUnionAnalyzer;

namespace ArchitectureDemo.Results;

[DiscriminatedUnion]
public abstract class CreateUserResult
{
    private CreateUserResult()
    {
    }

    public sealed class Created : CreateUserResult
    {
        public Created(UserId id)
        {
            Id = id;
        }

        public UserId Id { get; }
    }

    public sealed class EmailAlreadyRegistered : CreateUserResult
    {
    }

    public sealed class ParentNotFound : CreateUserResult
    {
    }
}
