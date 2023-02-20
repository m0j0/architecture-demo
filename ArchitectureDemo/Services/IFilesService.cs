using ArchitectureDemo.Results;
using ArchitectureDemo.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureDemo.Services
{
    public interface IFilesService
    {
        Task<GetFileResult> GetFile(UserId userId, FileId fileId, CancellationToken cancellationToken);

        Task<UploadFileResult> UploadFile(UserId userId, Stream file, string fileName, CancellationToken cancellationToken);
    }
}
