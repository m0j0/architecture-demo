using ArchitectureDemo.Results;
using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Services;

public interface IFilesService
{
    Task<GetFileResult> GetFile(UserId userId, FileId fileId, CancellationToken cancellationToken);

    Task<UploadFileResult> UploadFile(UserId userId, Stream file, string fileName, CancellationToken cancellationToken);
}
