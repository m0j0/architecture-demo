using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Models
{
    public sealed record UserFileModel(FileId Id, string Name);
}
