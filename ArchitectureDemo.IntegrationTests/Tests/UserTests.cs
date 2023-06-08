using ArchitectureDemo.gRPC.Services;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Xunit.Abstractions;

namespace ArchitectureDemo.IntegrationTests.Tests;

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
        using var gRpcChannel = factory.CreateGRpcChannel();
        var usersClient = new Users.UsersClient(gRpcChannel);

        //
        var createResponse = await usersClient.CreateAsync(
            new CreateRequest { Name = "Parent", Email = "a@b" });

        // 
        createResponse.ResultCase.Should().Be(CreateResponse.ResultOneofCase.UserId);
        var userId = createResponse.UserId;

        //
        var userResponse = await usersClient.GetByIdAsync(new GetByIdRequest { Id = userId });
        var user = userResponse.User;
        user.Should().NotBeNull();
        user!.Name.Should().BeEquivalentTo("Parent");
    }

    [Fact]
    public async Task CreateUser_DuplicateEmailTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        using var gRpcChannel = factory.CreateGRpcChannel();
        var usersClient = new Users.UsersClient(gRpcChannel);

        //
        var user1Response = await usersClient.CreateAsync(
            new CreateRequest { Name = "User1", Email = "a@b" });
        var user2Response = await usersClient.CreateAsync(
            new CreateRequest { Name = "User2", Email = "a@b" });

        // 
        user1Response.ResultCase.Should().Be(CreateResponse.ResultOneofCase.UserId);
        user2Response.ResultCase.Should().Be(CreateResponse.ResultOneofCase.EmailAlreadyRegistered);
    }

    [Fact]
    public async Task CreateUser_ParentNotFoundTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        using var gRpcChannel = factory.CreateGRpcChannel();
        var usersClient = new Users.UsersClient(gRpcChannel);

        //
        var createResponse = await usersClient.CreateAsync(
            new CreateRequest { Name = "User1", Email = "a@a", ParentId = 100200 });

        // 
        createResponse.ResultCase.Should().Be(CreateResponse.ResultOneofCase.ParentNotFound);
    }

    public static IEnumerable<object?[]> GetAllTestData =>
        new List<object?[]>
        {
            new object?[] { null, null, 11 },
            new object?[] { "Parent", null, 2 },
            new object?[] { "Child", DateTimeOffset.UtcNow.AddDays(-1), 9 },
            new object?[] { null, DateTimeOffset.UtcNow.AddDays(1), 0 },
            new object?[] { "somename", null, 0 },
        };

    [Theory]
    [MemberData(nameof(GetAllTestData))]
    public async Task GetAll_Test(string? name, DateTimeOffset? createdAfter, int expectedCount)
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        using var gRpcChannel = factory.CreateGRpcChannel();
        var usersClient = new Users.UsersClient(gRpcChannel);

        await InitializeDb(usersClient);

        //
        var usersResponse = await usersClient.GetAllAsync(new GetAllFilter()
        {
            Name = name,
            CreatedAfter = createdAfter == null
                ? null
                : Timestamp.FromDateTimeOffset(createdAfter.Value)
        });

        //
        var users = usersResponse.Users;
        users.Should().HaveCount(expectedCount);
    }

    [Fact]
    public async Task GetTree_Test()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        using var gRpcChannel = factory.CreateGRpcChannel();
        var usersClient = new Users.UsersClient(gRpcChannel);

        await InitializeDb(usersClient);

        //
        var usersResponse = await usersClient.GetTreeAsync(new Empty());

        //
        var users = usersResponse.Users;
        users.Should().HaveCount(2);
        var parent1 = users.Should().ContainSingle(u => u.Name == "Parent1").Subject;
        foreach (var child in parent1.Children.Should().HaveCount(3).And.Subject)
        {
            child.Children.Should().HaveCount(2);
        }

        users.Should().ContainSingle(u => u.Name == "Parent2").Which.Children.Should().HaveCount(0);
    }

    private static async Task InitializeDb(Users.UsersClient usersClient)
    {
        var parentResponse = await usersClient.CreateAsync(
            new CreateRequest { Name = "Parent1", Email = "a@b" });
        parentResponse.ResultCase.Should().Be(CreateResponse.ResultOneofCase.UserId);
        var parentId = parentResponse.UserId;

        for (int i = 0; i < 3; i++)
        {
            var childResponse = await usersClient.CreateAsync(
                new CreateRequest { Name = "Child" + i, Email = "a@b" + i, ParentId = parentId });
            childResponse.ResultCase.Should().Be(CreateResponse.ResultOneofCase.UserId);
            var childId = childResponse.UserId;

            for (int j = 0; j < 2; j++)
            {
                await usersClient.CreateAsync(
                    new CreateRequest
                    {
                        Name = "Child" + i + j,
                        Email = "a@b" + i + j,
                        ParentId = childId
                    });
            }
        }

        await usersClient.CreateAsync(
            new CreateRequest
            {
                Name = "Parent2",
                Email = "a@c"
            });
    }
}
