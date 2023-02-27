using System.Net.Http.Json;
using ArchitectureDemo.ValueObjects;
using ArchitectureDemo.WebApi.Host.Dtos;
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
    public async Task CreateUser_Scenario()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();
        var typedClient = new TypedClient(httpClient);

        //
        var parentResponse = await typedClient.CreateUserAsync("Parent", "a@b");

        // 
        parentResponse.ResponseTag.Should().Be(CreateUserResponse.Tag.UserCreated);
        var userId = parentResponse.UserId!.Value;

        //
        var user = await typedClient.GetUserAsync(userId);
        user.Should().NotBeNull();
        user!.Name.Should().BeEquivalentTo("Parent");
    }

    [Fact]
    public async Task CreateUser_DuplicateEmailTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();
        var typedClient = new TypedClient(httpClient);

        //
        var user1 = await typedClient.CreateUserAsync("User1", "a@b");
        var user2 = await typedClient.CreateUserAsync("User2", "a@b");

        // 
        user1.ResponseTag.Should().Be(CreateUserResponse.Tag.UserCreated);
        user2.ResponseTag.Should().Be(CreateUserResponse.Tag.EmailAlreadyRegistered);
    }

    [Fact]
    public async Task CreateUser_ParentNotFoundTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();
        var typedClient = new TypedClient(httpClient);

        //
        var user = await typedClient.CreateUserAsync("User", "a@a", 100200);

        // 
        user.ResponseTag.Should().Be(CreateUserResponse.Tag.ParentNotFound);
    }

    [Fact]
    public async Task GetAll_Test()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        await InitializeDb(httpClient);

        //
        var usersResponse = await httpClient.GetFromJsonAsync<GetAllUsersResponse>("api/users/getAll");

        //
        var users = usersResponse!.Users.ToArray();
        users.Should().HaveCount(11);
        users.Should().ContainSingle(u => u.Name == "Parent1");
    }

    [Fact]
    public async Task GetTree_Test()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        await InitializeDb(httpClient);

        //
        var usersResponse = await httpClient.GetFromJsonAsync<GetUsersTreeResponse>("api/users/getTree");

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

        var parentResponse = await typedClient.CreateUserAsync("Parent1", "a@b");
        parentResponse.ResponseTag.Should().Be(CreateUserResponse.Tag.UserCreated);
        var parentId = parentResponse.UserId;

        for (int i = 0; i < 3; i++)
        {
            var childResponse = await typedClient.CreateUserAsync("Child" + i, "a@b" + i, parentId);
            childResponse.ResponseTag.Should().Be(CreateUserResponse.Tag.UserCreated);
            var childId = childResponse.UserId;
            
            for (int j = 0; j < 2; j++)
            {
                await typedClient.CreateUserAsync("Child" + i + j, "a@b" + i + j, childId);
            }
        }
        
        await typedClient.CreateUserAsync("Parent2", "a@c");
    }
}
