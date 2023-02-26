using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.DAL.Entities;

internal class User
{
    public const string EmailUniqueIndexName = "user_email_key";

    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public User? Parent { get; set; }

    public List<User> Children { get; set; } = null!;

    public List<UserFile> Files { get; set; } = null!;
}
