namespace ArchitectureDemo.WebApi.Host.Dtos;

public class UploadFileResponse
{
    public UploadFileResponse(int fileId)
    {
        FileId = fileId;
    }

    public int FileId { get; }
}
