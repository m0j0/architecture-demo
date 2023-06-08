using System.Linq.Expressions;
using NeinLinq;

namespace ArchitectureDemo.DAL.Entities;

internal static class UserExtensions
{
    [InjectLambda]
    public static bool FilterByName(this User user, string? filter)
    {
        throw new NotImplementedException();
    }

    private static Expression<Func<User, string?, bool>> FilterByName()
    {
        return (user, filter) => string.IsNullOrEmpty(filter) || user.Name.Contains(filter);
    }

    [InjectLambda]
    public static bool FilterByCreationDate(this User user, DateTimeOffset? date)
    {
        throw new NotImplementedException();
    }

    private static Expression<Func<User, DateTimeOffset?, bool>> FilterByCreationDate()
    {
        return (user, date) => date == null || user.CreationDate >= date;
    }
}
