namespace ArchitectureDemo.WebApiHost.Dtos;

public class UploadFileResponse
{
    public enum Tag
    {
        Success,
        UserNotFound,
        FilesCountLimitExceeded
    }
    
    public Tag ResponseTag { get; set; }

    public Guid FileId { get; set; }

}
