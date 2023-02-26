using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.DAL.Entities;

internal class UserFile
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public User User { get; set; } = null!;
}
