using ArchitectureDemo.BL.Services;
using ArchitectureDemo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectureDemo.BL;

public static class BlServiceCollectionExtensions
{
    public static IServiceCollection AddArchitectureDemoBl(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IFilesService, FilesService>();

        return serviceCollection;
    }
}
