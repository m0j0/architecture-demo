using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.States;

public sealed record Success;

public sealed record UploadFileSuccess(FileId FileId); // TODO наименование

public sealed record FileNotFound;

public sealed record UserNotFound;

public sealed record LockAcquired(IAsyncDisposable Lock);

public sealed record FilesCountLimitExceeded;

public sealed record UserCreated(UserId Id);

public sealed record EmailAlreadyRegistered;
