using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ArchitectureDemo.WebApiHost.Dtos;
using FluentAssertions;

namespace ArchitectureDemo.IntegrationTests
{
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
                .ReadFromJsonAsync<WebApiHost.Dtos.GetUserResponse>();
            response.Should().NotBeNull();
            return response!.User;
        }

        public async Task<CreateUserResponse> CreateUserAsync(string name, Guid? parentId = null)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync("api/users/create",
                new WebApiHost.Dtos.CreateUserRequest { Name = name, ParentId = parentId }
            );
            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.Content
                .ReadFromJsonAsync<WebApiHost.Dtos.CreateUserResponse>();
            response.Should().NotBeNull();
            return response!;
        }
    }
}
