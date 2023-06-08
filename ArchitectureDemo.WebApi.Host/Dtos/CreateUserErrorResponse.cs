namespace ArchitectureDemo.WebApi.Host.Dtos;

public class CreateUserErrorResponse
{
    public CreateUserErrorResponse(CreateUserErrorCode code)
    {
        Code = code;
    }

    public CreateUserErrorCode Code { get; }
}

public enum CreateUserErrorCode
{
    EmailAlreadyRegistered,
    ParentNotFound
}
