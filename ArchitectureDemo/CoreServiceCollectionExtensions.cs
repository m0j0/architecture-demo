using ArchitectureDemo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectureDemo;

public static class CoreServiceCollectionExtensions
{
    public static IServiceCollection AddArchitectureDemoCore(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IFilesService, FilesService>();

        return serviceCollection;
    }
}
