using DiscriminatedUnionAnalyzer;

namespace ArchitectureDemo.Results;

[DiscriminatedUnion]
public abstract class GetFileResult
{
    private GetFileResult()
    {
    }

    public sealed class FileStream : GetFileResult
    {
        public FileStream(Stream stream)
        {
            Stream = stream;
        }

        public Stream Stream { get; }
    }

    public sealed class UserNotFound : GetFileResult
    {
    }

    public sealed class FileNotFound : GetFileResult
    {
    }
}
