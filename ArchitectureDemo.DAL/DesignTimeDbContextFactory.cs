using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureDemo.DAL;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DemoContext>
{
    public DemoContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoContext>();

        DemoContext.ConfigureDbContextOptionsBuilder(
            optionsBuilder,
            "Server=localhost;Port=5432;Database=DemoDb;User Id=postgres;Password=postgres;"
        );

        return new DemoContext(optionsBuilder.Options);
    }
}
