using ArchitectureDemo;
using ArchitectureDemo.DAL;
using ArchitectureDemo.S3;
using ArchitectureDemo.Settings;
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

builder.Services
    .AddArchitectureDemoCore()
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
