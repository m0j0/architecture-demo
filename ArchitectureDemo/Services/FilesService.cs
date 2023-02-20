using ArchitectureDemo.Repositories;
using ArchitectureDemo.Results;
using ArchitectureDemo.States;
using ArchitectureDemo.ValueObjects;

namespace ArchitectureDemo.Services;

internal class FilesService : IFilesService
{
    private const int MaxUserFilesCount = 3;

    private readonly IUsersRepository _usersRepository;
    private readonly IS3Service _s3Service;

    public FilesService(IUsersRepository usersRepository, IS3Service s3Service)
    {
        _usersRepository = usersRepository;
        _s3Service = s3Service;
    }

    public async Task<GetFileResult> GetFile(UserId userId, FileId fileId, CancellationToken cancellationToken)
    {
        if (!await _usersRepository.DoesUserExist(userId, cancellationToken))
        {
            return new UserNotFound();
        }

        var userFile = await _usersRepository.GetFile(userId, fileId, cancellationToken);
        if (userFile == null)
        {
            return new FileNotFound();
        }

        return await _s3Service.GetFile(userFile.Name, cancellationToken);
    }

    public async Task<UploadFileResult> UploadFile(UserId userId, Stream file, string fileName, CancellationToken cancellationToken)
    {
        await using var lockResult = await _usersRepository.LockUser(userId, cancellationToken);

        return await lockResult
            .MatchAsync(
                async acquiredLock =>
                {
                    var userFilesCount = await _usersRepository.GetUserFilesCount(userId, cancellationToken);
                    if (userFilesCount >= MaxUserFilesCount)
                    {
                        return new FilesCountLimitExceeded();
                    }

                    var fileId = await _usersRepository.AddFileToUser(userId, fileName, cancellationToken);

                    await _s3Service.UploadFile(file, fileName, cancellationToken);

                    return new UploadFileSuccess(fileId);
                },
                userNotFound => Task.FromResult<UploadFileResult>(userNotFound));
    }
}
