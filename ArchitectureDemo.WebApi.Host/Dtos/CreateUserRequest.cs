using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.WebApi.Host.Dtos;

public class CreateUserRequest
{
    public string Name { get; set; } = null!;

    [EmailAddress]
    public string Email { get; set; } = null!;

    public int? ParentId { get; set; }
}
