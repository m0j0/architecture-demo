using ArchitectureDemo.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscriminatedUnionGenerator;

namespace ArchitectureDemo.Results
{
    [DiscriminatedUnionCase(typeof(UploadFileSuccess))]
    [DiscriminatedUnionCase(typeof(UserNotFound))]
    [DiscriminatedUnionCase(typeof(FilesCountLimitExceeded))]
    public sealed partial record UploadFileResult;
}
