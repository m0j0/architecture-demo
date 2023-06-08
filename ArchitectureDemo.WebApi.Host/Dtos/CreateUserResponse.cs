namespace ArchitectureDemo.WebApi.Host.Dtos;

public class CreateUserResponse
{
    public CreateUserResponse(int userId)
    {
        UserId = userId;
    }

    public int UserId { get; }
}
