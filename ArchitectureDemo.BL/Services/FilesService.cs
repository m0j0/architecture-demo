using ArchitectureDemo.Repositories;
using ArchitectureDemo.Results;
using ArchitectureDemo.Services;
using ArchitectureDemo.ValueObjects;
using System.Runtime.CompilerServices;

namespace ArchitectureDemo.BL.Services;

internal class FilesService : IFilesService
{
    private const int MaxUserFilesCount = 3;

    private readonly IUsersRepository _usersRepository;
    private readonly IS3Service _s3Service;
    private readonly ILockService _lockService;

    public FilesService(IUsersRepository usersRepository, IS3Service s3Service, ILockService lockService)
    {
        _usersRepository = usersRepository;
        _s3Service = s3Service;
        _lockService = lockService;
    }

    public async Task<GetFileResult> GetFile(UserId userId, FileId fileId,
        CancellationToken cancellationToken)
    {
        if (!await _usersRepository.DoesUserExist(userId, cancellationToken))
        {
            return new GetFileResult.UserNotFound();
        }

        var fileName = await _usersRepository.GetFileName(fileId, cancellationToken);
        if (fileName == null)
        {
            return new GetFileResult.FileNotFound();
        }

        return new GetFileResult.FileStream(
            await _s3Service.GetFile(fileName, cancellationToken));
    }

    public async Task<UploadFileResult> UploadFile(
        UserId userId, Stream file, string fileName, CancellationToken cancellationToken)
    {
        if (!await _usersRepository.DoesUserExist(userId, cancellationToken))
        {
            return new UploadFileResult.UserNotFound();
        }

        var lockResult = await _lockService
            .Acquire(LockConstants.User, userId.Value, cancellationToken);

        switch (lockResult)
        {
            case LockResult.Acquired acquired:
                await using (acquired.Lock)
                {
                    var userFilesCount = await _usersRepository
                        .GetUserFilesCount(userId, cancellationToken);
                    if (userFilesCount >= MaxUserFilesCount)
                    {
                        return new UploadFileResult.FilesCountLimitExceeded();
                    }

                    var fileId = await _usersRepository
                        .AddFileToUser(userId, fileName, cancellationToken);
                    await _s3Service.UploadFile(file, fileName, cancellationToken);
                    return new UploadFileResult.UploadedSuccessfully(fileId);
                }

            case LockResult.AlreadyLocked:
                return new UploadFileResult.AlreadyLocked();

            default:
                throw new SwitchExpressionException();
        }
    }
}
