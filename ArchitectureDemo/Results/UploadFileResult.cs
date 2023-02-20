using ArchitectureDemo.States;
using DiscriminatedUnionGenerator;

namespace ArchitectureDemo.Results;

[DiscriminatedUnionCase(typeof(UploadFileSuccess))]
[DiscriminatedUnionCase(typeof(UserNotFound))]
[DiscriminatedUnionCase(typeof(FilesCountLimitExceeded))]
public sealed partial record UploadFileResult;
