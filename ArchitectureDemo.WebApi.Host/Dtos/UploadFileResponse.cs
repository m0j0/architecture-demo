namespace ArchitectureDemo.WebApi.Host.Dtos;

public class UploadFileResponse
{
    public enum Tag
    {
        Success,
        UserNotFound,
        FilesCountLimitExceeded
    }
    
    public Tag ResponseTag { get; set; }

    public int FileId { get; set; }

}
