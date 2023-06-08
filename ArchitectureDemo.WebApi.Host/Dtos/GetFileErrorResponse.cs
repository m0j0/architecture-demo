namespace ArchitectureDemo.WebApi.Host.Dtos;

public class GetFileErrorResponse
{
    public GetFileErrorResponse(GetFileErrorCode code)
    {
        Code = code;
    }

    public GetFileErrorCode Code { get; }
}

public enum GetFileErrorCode
{
    UserNotFound,
    FileNotFound
}
