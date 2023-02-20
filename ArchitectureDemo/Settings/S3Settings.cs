using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.Settings;

public sealed class S3Settings
{
    [Required]
    public string ServiceUrl { get; set; }

    [Required]
    public string AccessKey { get; set; }

    [Required]
    public string SecretKey { get; set; }
}
