using ArchitectureDemo.DAL.Repositories;
using ArchitectureDemo.DAL.Services;
using ArchitectureDemo.Repositories;
using ArchitectureDemo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectureDemo.DAL;

public static class DalServiceCollectionExtensions
{
    public static IServiceCollection AddArchitectureDemoDal(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContextPool<DemoContext>((provider, optionsBuilder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();

            optionsBuilder
                .UseNpgsql(configuration.GetConnectionString("DemoDb"))
                .UseSnakeCaseNamingConvention();

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif
        });

        serviceCollection.AddScoped<IUsersService, UsersService>();
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();

        return serviceCollection;
    }
}
