using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.DAL.Entities;

internal class UserFile
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public User User { get; set; } = null!;
}
