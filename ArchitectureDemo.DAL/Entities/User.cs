using System.ComponentModel.DataAnnotations;

namespace ArchitectureDemo.DAL.Entities;

internal class User
{
    public const string EmailUniqueIndexName = "users_email_key";

    public const string ParentIdForeignKeyName = "users_parent_id_fkey";

    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? ParentId { get; set; }

    public DateTimeOffset CreationDate { get; set; }

    public User? Parent { get; set; }

    public List<User> Children { get; set; } = null!;

    public List<UserFile> Files { get; set; } = null!;
}
