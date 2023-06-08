namespace ArchitectureDemo.WebApi.Host.Dtos;

public class UploadFileErrorResponse
{
    public UploadFileErrorResponse(UploadFileErrorCode code)
    {
        Code = code;
    }

    public UploadFileErrorCode Code { get; }
}

public enum UploadFileErrorCode
{
    UserNotFound,
    FilesCountLimitExceeded,
    AlreadyLocked
}
