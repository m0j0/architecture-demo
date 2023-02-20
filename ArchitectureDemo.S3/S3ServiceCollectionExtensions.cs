using Amazon.S3;
using ArchitectureDemo.S3.Services;
using ArchitectureDemo.Services;
using ArchitectureDemo.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ArchitectureDemo.S3;

public static class S3ServiceCollectionExtensions
{
    public static IServiceCollection AddArchitectureDemoS3(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAmazonS3, AmazonS3Client>(provider =>
        {
            var settings = provider.GetRequiredService<IOptionsSnapshot<S3Settings>>().Value;
            return new AmazonS3Client(settings.AccessKey,
                settings.SecretKey,
                new AmazonS3Config
                {
                    ServiceURL = settings.ServiceUrl,
                    ForcePathStyle = true, // MUST be true to work correctly with MinIO server
                    RetryMode = Amazon.Runtime.RequestRetryMode.Adaptive
                });
        });

        serviceCollection.AddScoped<IS3Service, S3Service>();

        return serviceCollection;
    }
}
