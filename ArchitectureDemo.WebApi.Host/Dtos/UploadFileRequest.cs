using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.WebApiHost.Dtos;

public class UploadFileRequest
{
    [Required]
    public Guid? UserId { get; set; }

    [Required]
    public IFormFile? File { get; set; }
}
