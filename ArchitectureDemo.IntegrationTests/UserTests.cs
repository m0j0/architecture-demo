using System.Net.Http.Json;
using ArchitectureDemo.WebApiHost.Dtos;
using FluentAssertions;
using Xunit.Abstractions;

namespace ArchitectureDemo.IntegrationTests;

public class UserTests : IClassFixture<TestContext>
{
    private readonly TestContext _context;
    private readonly ITestOutputHelper _testOutputHelper;

    public UserTests(TestContext context, ITestOutputHelper testOutputHelper)
    {
        _context = context;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetAllTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        await InitializeDb(httpClient);

        //
        var usersResponse = await httpClient.GetFromJsonAsync<WebApiHost.Dtos.GetAllUsersResponse>("api/users/getAll");

        //
        var users = usersResponse!.Users.ToArray();
        users.Should().HaveCount(11);
        users.Should().ContainSingle(u => u.Name == "Parent1");
    }

    [Fact]
    public async Task GetTreeTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        await InitializeDb(httpClient);

        //
        var usersResponse = await httpClient.GetFromJsonAsync<WebApiHost.Dtos.GetUsersTreeResponse>("api/users/getTree");

        //
        var users = usersResponse!.Users.ToArray();
        users.Should().HaveCount(2);
        var parent1 = users.Should().ContainSingle(u => u.Name == "Parent1").Subject;
        foreach (var child in parent1.Children.Should().HaveCount(3).And.Subject)
        {
            child.Children.Should().HaveCount(2);
        }

        users.Should().ContainSingle(u => u.Name == "Parent2").Which.Children.Should().HaveCount(0);
    }

    private static async Task InitializeDb(HttpClient httpClient)
    {
        var typedClient = new TypedClient(httpClient);

        var parentResponse = await typedClient.CreateUserAsync("Parent1");
        parentResponse.ResponseTag.Should().Be(CreateUserResponse.Tag.UserCreated);
        var parentId = parentResponse.UserId;

        for (int i = 0; i < 3; i++)
        {
            var childResponse = await typedClient.CreateUserAsync("Child" + i, parentId);
            childResponse.ResponseTag.Should().Be(CreateUserResponse.Tag.UserCreated);
            var childId = childResponse.UserId;
            
            for (int j = 0; j < 2; j++)
            {
                await typedClient.CreateUserAsync("Child" + i + j, childId);
            }
        }
        
        await typedClient.CreateUserAsync("Parent2");
    }
}
