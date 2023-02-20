namespace ArchitectureDemo.Services;

public interface IS3Service
{
    Task<Stream> GetFile(string fileName, CancellationToken cancellationToken);

    Task UploadFile(Stream file, string fileName, CancellationToken cancellationToken);
}
