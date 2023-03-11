using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ArchitectureDemo.IntegrationTests.Unit;

public class UsersServiceTests : IClassFixture<TestContext>
{
    private readonly TestContext _context;
    private readonly ITestOutputHelper _testOutputHelper;

    public UsersServiceTests(TestContext context, ITestOutputHelper testOutputHelper)
    {
        _context = context;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateUser_Scenario()
    {
        var dbContext = await _context.CreateDb(_testOutputHelper);

        await dbContext.UserFiles.ToArrayAsync();
    }
}
