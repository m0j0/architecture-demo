using ArchitectureDemo.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscriminatedUnionGenerator;

namespace ArchitectureDemo.Results
{
    [DiscriminatedUnionCase(typeof(Stream), "FileStream")]
    [DiscriminatedUnionCase(typeof(UserNotFound))]
    [DiscriminatedUnionCase(typeof(FileNotFound))]
    public sealed partial record GetFileResult;
}
