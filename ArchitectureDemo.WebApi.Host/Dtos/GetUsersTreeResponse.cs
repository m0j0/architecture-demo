namespace ArchitectureDemo.WebApi.Host.Dtos;

public class GetUsersTreeResponse
{
    public GetUsersTreeResponse(IEnumerable<UserWithChildrenDto> users)
    {
        Users = users;
    }

    public IEnumerable<UserWithChildrenDto> Users { get; }
}
