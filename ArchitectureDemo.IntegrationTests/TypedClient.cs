using System.Net.Http.Json;
using ArchitectureDemo.WebApi.Host.Dtos;
using FluentAssertions;

namespace ArchitectureDemo.IntegrationTests;

internal class TypedClient
{
    private readonly HttpClient _httpClient;

    public TypedClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserDto?> GetUserAsync(Guid id)
    {
        var responseMessage = await _httpClient.GetAsync(
            $"api/users/getById?id={id}"
        );
        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content
            .ReadFromJsonAsync<GetUserResponse>();
        response.Should().NotBeNull();
        return response!.User;
    }

    public async Task<CreateUserResponse> CreateUserAsync(string name, string email, Guid? parentId = null)
    {
        var responseMessage = await _httpClient.PostAsJsonAsync("api/users/create",
            new CreateUserRequest { Name = name, Email = email, ParentId = parentId }
        );
        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content
            .ReadFromJsonAsync<CreateUserResponse>();
        response.Should().NotBeNull();
        return response!;
    }
}
