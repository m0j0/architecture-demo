using ArchitectureDemo.BL;
using ArchitectureDemo.DAL;
using ArchitectureDemo.Infrastructure;
using ArchitectureDemo.S3;
using ArchitectureDemo.Settings;
using ArchitectureDemo.WebApi.Host.Infrastructure;
using Microsoft.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<S3Settings>()
    .Bind(builder.Configuration.GetSection(nameof(S3Settings)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<RecyclableMemoryStreamManager>();
builder.Services.AddSingleton<ISystemClock, SystemClock>();

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

app.Run();

public partial class Program
{
}
