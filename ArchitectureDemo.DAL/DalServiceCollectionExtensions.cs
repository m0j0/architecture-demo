using ArchitectureDemo.DAL.Repositories;
using ArchitectureDemo.DAL.Services;
using ArchitectureDemo.Repositories;
using ArchitectureDemo.Services;
using ArchitectureDemo.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ArchitectureDemo.DAL;

public static class DalServiceCollectionExtensions
{
    public static IServiceCollection AddArchitectureDemoDal(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContextPool<DemoContext>((provider, optionsBuilder) =>
        {
            var options = provider.GetRequiredService<IOptionsMonitor<ConnectionStringSettings>>();

            DemoContext.ConfigureDbContextOptionsBuilder(optionsBuilder, options.CurrentValue.DemoDb);
        });

        serviceCollection.AddScoped<IUsersService, UsersService>();
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
        serviceCollection.AddScoped<ILockService, LockService>();

        return serviceCollection;
    }
}
