namespace ArchitectureDemo.WebApi.Host.Dtos;

public class CreateUserResponse
{
    public enum Tag
    {
        UserCreated,
        EmailAlreadyRegistered,
        ParentNotFound
    }

    public Tag ResponseTag { get; set; }

    public int? UserId { get; set; }
}
