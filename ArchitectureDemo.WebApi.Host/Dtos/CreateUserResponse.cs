namespace ArchitectureDemo.WebApi.Host.Dtos;

public class CreateUserResponse
{
    public enum Tag
    {
        UserCreated,
        EmailAlreadyRegistered
    }

    public Tag ResponseTag { get; set; }

    public Guid UserId { get; set; }
}
