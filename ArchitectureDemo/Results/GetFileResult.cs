using ArchitectureDemo.States;
using DiscriminatedUnionGenerator;

namespace ArchitectureDemo.Results;

[DiscriminatedUnionCase(typeof(Stream), "FileStream")]
[DiscriminatedUnionCase(typeof(UserNotFound))]
[DiscriminatedUnionCase(typeof(FileNotFound))]
public sealed partial record GetFileResult;
