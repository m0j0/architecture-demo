using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.States;

public sealed record Success;

public sealed record UploadFileSuccess(FileId FileId); // TODO наименование

public sealed record FileNotFound;

public sealed record UserNotFound;

public sealed record LockAcquired(IAsyncDisposable Lock);

public sealed record AlreadyLocked;

public sealed record FilesCountLimitExceeded;

public sealed record UserCreated(UserId Id);

public sealed record EmailAlreadyRegistered;

public sealed record ParentNotFound;
