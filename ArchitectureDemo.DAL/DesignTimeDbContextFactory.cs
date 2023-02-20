using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureDemo.DAL;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DemoContext>
{
    public DemoContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoContext>();
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=DemoDb;User Id=postgres;Password=postgres;").UseSnakeCaseNamingConvention();

        return new DemoContext(optionsBuilder.Options);
    }
}
