using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchitectureDemo.States;
using DiscriminatedUnionGenerator;

namespace ArchitectureDemo.Results
{
    [DiscriminatedUnionCase(typeof(UserCreated))]
    [DiscriminatedUnionCase(typeof(EmailAlreadyRegistered))]
    public sealed partial record CreateUserResult;
}
