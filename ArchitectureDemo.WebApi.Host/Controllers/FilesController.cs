using System.Net.Mime;
using System.Runtime.CompilerServices;
using ArchitectureDemo.Results;
using ArchitectureDemo.Services;
using ArchitectureDemo.ValueObjects;
using ArchitectureDemo.WebApi.Host.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IO;

namespace ArchitectureDemo.WebApi.Host.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IFilesService _filesService;
    private readonly RecyclableMemoryStreamManager _streamManager;

    public FilesController(IFilesService filesService, RecyclableMemoryStreamManager streamManager)
    {
        _filesService = filesService;
        _streamManager = streamManager;
    }

    [HttpPost("uploadUserFile")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadFileResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(UploadFileErrorResponse))]
    public async Task<ActionResult<UploadFileResponse>> UploadFile(
        [FromForm] UploadFileRequest request, CancellationToken cancellationToken)
    {
        var requestFile = request.File!;
        using var file = await BufferFileAsync(requestFile, cancellationToken);

        var result = await _filesService.UploadFile
            (new UserId(request.UserId!.Value), file, requestFile.FileName, cancellationToken);

        return result switch
        {
            UploadFileResult.UploadedSuccessfully success =>
                Ok(new UploadFileResponse(success.FileId.Value)),
            UploadFileResult.UserNotFound =>
                Conflict(new UploadFileErrorResponse(UploadFileErrorCode.UserNotFound)),
            UploadFileResult.FilesCountLimitExceeded =>
                Conflict(new UploadFileErrorResponse(UploadFileErrorCode.FilesCountLimitExceeded)),
            UploadFileResult.AlreadyLocked =>
                Conflict(new UploadFileErrorResponse(UploadFileErrorCode.AlreadyLocked)),
            _ => throw new SwitchExpressionException()
        };
    }

    [HttpGet("getFile")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(GetFileErrorResponse))]
    public async Task<IActionResult> GetFile(
        int userId, int fileId, CancellationToken cancellationToken)
    {
        var getFileResult = await _filesService.GetFile(new UserId(userId), new FileId(fileId), cancellationToken);

        return getFileResult switch
        {
            GetFileResult.FileStream fileStream =>
                File(fileStream.Stream, MediaTypeNames.Application.Octet),
            GetFileResult.UserNotFound =>
                Conflict(new GetFileErrorResponse(GetFileErrorCode.UserNotFound)),
            GetFileResult.FileNotFound =>
                Conflict(new GetFileErrorResponse(GetFileErrorCode.FileNotFound)),
            _ => throw new SwitchExpressionException()
        };
    }

    private async ValueTask<MemoryStream> BufferFileAsync(IFormFile formFile, CancellationToken ct)
    {
        var mem = _streamManager.GetStream();
        await formFile.CopyToAsync(mem, ct);
        mem.Position = 0;
        return mem;
    }
}
