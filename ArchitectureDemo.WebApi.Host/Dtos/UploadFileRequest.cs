using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.WebApi.Host.Dtos;

public class UploadFileRequest
{
    [Required]
    public int? UserId { get; set; }

    [Required]
    public IFormFile? File { get; set; }
}
