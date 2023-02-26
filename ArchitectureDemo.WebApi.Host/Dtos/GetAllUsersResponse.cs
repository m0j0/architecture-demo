namespace ArchitectureDemo.WebApiHost.Dtos;

public class GetAllUsersResponse
{
    public GetAllUsersResponse(IEnumerable<UserDto> users)
    {
        Users = users;
    }

    public IEnumerable<UserDto> Users { get;  }
}
