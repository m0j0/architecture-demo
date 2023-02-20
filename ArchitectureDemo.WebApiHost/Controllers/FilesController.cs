using System.Net.Mime;
using ArchitectureDemo.Services;
using ArchitectureDemo.ValueObjects;
using ArchitectureDemo.WebApiHost.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IO;

namespace ArchitectureDemo.WebApiHost.Controllers;

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
    public async Task<ActionResult<UploadFileResponse>> UploadFile([FromForm] UploadFileRequest request,
        CancellationToken cancellationToken)
    {
        var requestFile = request.File!;
        using var file = await BufferFileAsync(requestFile, cancellationToken);

        var result = await _filesService.UploadFile
            (new UserId(request.UserId!.Value), file, requestFile.FileName, cancellationToken);

        return result.Match<ActionResult<UploadFileResponse>>(
            success => Ok(new UploadFileResponse { ResponseTag = UploadFileResponse.Tag.Success, FileId = success.FileId.Value }),
            userNotFound => Ok(new UploadFileResponse { ResponseTag = UploadFileResponse.Tag.UserNotFound }),
            limitExceeded => Ok(new UploadFileResponse { ResponseTag = UploadFileResponse.Tag.FilesCountLimitExceeded })
        );
    }

    [HttpGet("getFile")]
    public async Task<IActionResult> GetFile(Guid userId, Guid fileId, CancellationToken cancellationToken)
    {
        var getFileResult = await _filesService.GetFile(new UserId(userId), new FileId(fileId), cancellationToken);

        return getFileResult.Match<IActionResult>(
            stream => File(stream, MediaTypeNames.Application.Octet),
            userNotFound => Ok(new GetFileResponse { ResponseTag = GetFileResponse.Tag.UserNotFound }),
            fileNotFound => Ok(new GetFileResponse { ResponseTag = GetFileResponse.Tag.FileNotFound })
        );
    }

    private async ValueTask<MemoryStream> BufferFileAsync(IFormFile formFile, CancellationToken ct)
    {
        var mem = _streamManager.GetStream();
        await formFile.CopyToAsync(mem, ct);
        mem.Position = 0;
        return mem;
    }
}
