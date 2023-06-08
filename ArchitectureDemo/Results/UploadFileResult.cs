using ArchitectureDemo.ValueObjects;
using DiscriminatedUnionAnalyzer;

namespace ArchitectureDemo.Results;

[DiscriminatedUnion]
public abstract class UploadFileResult
{
    private UploadFileResult()
    {
    }

    public sealed class UploadedSuccessfully : UploadFileResult
    {
        public UploadedSuccessfully(FileId fileId)
        {
            FileId = fileId;
        }

        public FileId FileId { get; }
    }

    public sealed class UserNotFound : UploadFileResult
    {
    }

    public sealed class FilesCountLimitExceeded : UploadFileResult
    {
    }

    public sealed class AlreadyLocked : UploadFileResult
    {
    }
}
