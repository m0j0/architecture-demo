using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.Settings;

public sealed class ConnectionStringSettings
{
    [Required]
    public string DemoDb { get; set; } = null!;
}
