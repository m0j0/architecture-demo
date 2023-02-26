using ArchitectureDemo.DAL;
using ArchitectureDemo.WebApi.Host;
using ComposeTestEnvironment.xUnit;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

[assembly: Xunit.TestFramework("ComposeTestEnvironment.xUnit.TestFramework", "ComposeTestEnvironment.xUnit")]

namespace ArchitectureDemo.IntegrationTests;

public class TestContext : DockerComposeEnvironmentFixture<ComposeDescriptor>
{
    public TestContext(IMessageSink output)
        : base(output)
    {
    }

    public async Task<WebApplicationFactory<Program>> CreateWebApplicationFactory(ITestOutputHelper testOutputHelper)
    {
        var pgConnectionString = Discovery.Substitute(
            "Server=$(pg.host);Port=$(pg.5432);Database=DemoDb;User Id=postgres;Password=postgres;");
        testOutputHelper.WriteLine($"PG conn string: {pgConnectionString}");

        var minioService = Discovery.Substitute("http://127.0.0.1:$(minio.9000)");
        var minioConsole = Discovery.Substitute("http://127.0.0.1:$(minio.9001)");
        testOutputHelper.WriteLine($"minioService: {minioService}, minioConsole: {minioConsole}");

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(loggingBuilder =>
                    loggingBuilder.Services.AddSingleton<ILoggerProvider>(_ => new XUnitLoggerProvider(testOutputHelper))
                );

                builder.ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.AddInMemoryCollection(
                        new Dictionary<string, string?>
                        {
                            ["S3Settings:ServiceUrl"] = minioService,
                            ["S3Settings:AccessKey"] = "myuserserviceaccount",
                            ["S3Settings:SecretKey"] = "myuserserviceaccountpassword",
                            ["ConnectionStrings:DemoDb"] = pgConnectionString
                        });
                });
            });

        await using var scope = factory.Services.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<DemoContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        return factory;
    }
}

public class ComposeDescriptor : DockerComposeDescriptor
{
    public override string ProjectName => "architecture-demo-integration-tests";

    public override string ComposeFileName => "docker-compose.yml";

    public override IReadOnlyDictionary<string, int[]> Ports => new Dictionary<string, int[]>
    {
        ["pg"] = new[] { 5432 },
        ["minio"] = new[] { 9000, 9001 }
    };

    public override IReadOnlyList<string> StartedMessageMarkers => new[]
    {
        "PostgreSQL init process complete; ready for start up",
        "Documentation: https://min.io/docs/minio/linux/index.html",
        "Access key successfully created"
    };
}
