var colorDisplayName = Color.Green.GetAttribute<DisplayAttribute>();

var colorDisplayName = Color.Green.GetDisplayName();

static string GetDisplayName(this Color color)
{
    return color switch
    {
        Color.Red => "Красный",
        Color.Green => "Зеленый",
        Color.Blue => "Синий",
        _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
    };
}

public enum Color
{
    Red,
    Green,
    Blue
}

public enum Color
{
    [Display(Name = "Красный")]
    Red,

    [Display(Name = "Зеленый")]
    Green,

    [Display(Name = "Синий")]
    Blue
}

public static class Extensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) 
            where TAttribute : Attribute
    {
        return enumValue.GetType()
                        .GetMember(enumValue.ToString())
                        .First()
                        .GetCustomAttribute<TAttribute>();
    }
}


public static class ColorExtensions
{
    public static string GetDisplayName(this Color color)
    {
        return color switch
        {
            Color.Red => "Красный",
            Color.Green => "Зеленый",
            Color.Blue => "Синий",
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }
}

interface IRepository<TEntity>
{
    IQueryable<TEntity> Query();
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}

interface IUnitOfWork
{
    Task SaveChanges();
}

throw new UserNotFoundException();

var query = db.Master.Select(x => new
{
    x.Id1,
    Details = x.Details.Select(d => d.DetailValue)
}).FirstOrDefault(x => x.Id1 == 3);

var users = await _demoContext
    .Users
    .ToInjectable()
    .Where(u => u.FilterByName(filter.Name) &&
                u.FilterByCreationDate(filter.CreatedAfter))
    .Select(u => new UserModel(new UserId(u.Id),
        u.Name,
        u.Files.Count,
        u.Parent == null ? null : new UserId(u.Parent!.Id),
        u.Parent == null ? null : u.Parent.Name))
    .ToArrayAsync(cancellationToken);


namespace Amazon.S3;
public interface IAmazonS3 : IDisposable, ICoreAmazonS3, IAmazonService
{
    Task<GetObjectResponse> GetObjectAsync(GetObjectRequest request, CancellationToken cancellationToken);
}

namespace ArchitectureDemo.Services;
public interface IS3Service
{
    Task<Stream> GetFile(string fileName, CancellationToken cancellationToken);
}
