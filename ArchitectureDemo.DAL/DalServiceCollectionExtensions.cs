using ArchitectureDemo.DAL.Repositories;
using ArchitectureDemo.DAL.Services;
using ArchitectureDemo.Repositories;
using ArchitectureDemo.Services;
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
            var connectionString = configuration.GetConnectionString("DemoDb") ??
                                   throw new InvalidOperationException();

            DemoContext.ConfigureDbContextOptionsBuilder(optionsBuilder, connectionString);
        });

        serviceCollection.AddScoped<IUsersService, UsersService>();
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
        serviceCollection.AddScoped<ILockService, LockService>();

        return serviceCollection;
    }
}
