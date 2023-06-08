using Microsoft.AspNetCore.Mvc.Testing;
using Grpc.Net.Client;

namespace ArchitectureDemo.IntegrationTests;

internal static class Extensions
{
    public static GrpcChannel CreateGRpcChannel(this WebApplicationFactory<Program> factory)
    {
        return GrpcChannel.ForAddress(factory.Server.BaseAddress,
            new GrpcChannelOptions
            {
                HttpHandler = factory.Server.CreateHandler()
            });
    }
}
