using ArchitectureDemo.BL;
using ArchitectureDemo.DAL;
using ArchitectureDemo.S3;
using ArchitectureDemo.Settings;
using ArchitectureDemo.WebApi.Host.gRPC.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IO;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

builder.Services.AddControllers();
builder.Services.AddGrpc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<S3Settings>()
    .Bind(builder.Configuration.GetSection(nameof(S3Settings)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<ConnectionStringSettings>()
    .Bind(builder.Configuration.GetSection("ConnectionStrings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<RecyclableMemoryStreamManager>();

builder.Services
    .AddArchitectureDemoBl()
    .AddArchitectureDemoDal()
    .AddArchitectureDemoS3();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGrpcService<UsersService>();

app.Run();

public partial class Program
{
}
