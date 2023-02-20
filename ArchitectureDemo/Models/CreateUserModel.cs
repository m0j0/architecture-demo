using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureDemo.Models
{
    public sealed record CreateUserModel(string Name, Guid? ParentId);
}
