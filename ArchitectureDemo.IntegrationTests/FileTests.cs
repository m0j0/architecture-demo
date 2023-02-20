using System.Net.Http.Json;
using ArchitectureDemo.WebApiHost.Dtos;
using FluentAssertions;
using Xunit.Abstractions;

namespace ArchitectureDemo.IntegrationTests;

public class FileTests : IClassFixture<TestContext>
{
    private readonly TestContext _context;
    private readonly ITestOutputHelper _testOutputHelper;

    public FileTests(TestContext context, ITestOutputHelper testOutputHelper)
    {
        _context = context;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task UploadFileScenario()
    {
        const string filename = "appsettings.pdf";

        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        var userId = await CreateUser(httpClient);

        var sourceFile = await File.ReadAllBytesAsync("appsettings.json");

        //
        var requestContent = new MultipartFormDataContent();

        var fileContent = new ByteArrayContent(sourceFile);
        requestContent.Add(fileContent, "file", filename);

        requestContent.Add(new StringContent(userId.ToString()), "userId");

        var uploadFileResponseMessage = await httpClient.PostAsync("api/files/uploadUserFile", requestContent);
        
        //
        uploadFileResponseMessage.IsSuccessStatusCode.Should().BeTrue();

        var uploadFileResponse = await uploadFileResponseMessage.Content.ReadFromJsonAsync<UploadFileResponse>();
        uploadFileResponse.Should().NotBeNull();
        uploadFileResponse!.ResponseTag.Should().Be(UploadFileResponse.Tag.Success);

        var fileId = uploadFileResponse.FileId;

        //
        var downloadedFileResponse = await httpClient.GetAsync($"api/files/getFile?userId={userId}&fileId={fileId}");
        
        downloadedFileResponse.IsSuccessStatusCode.Should().BeTrue();

        var downloadedFile = await downloadedFileResponse.Content.ReadAsByteArrayAsync();

        downloadedFile.Should().BeEquivalentTo(sourceFile);
    }

    [Fact]
    public async Task GetFile_UserNotFound()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        //
        var getFileResponseMessage = await httpClient.GetAsync($"api/files/getFile?userId={Guid.NewGuid()}&fileId={Guid.NewGuid()}");

        //
        getFileResponseMessage.IsSuccessStatusCode.Should().BeTrue();

        var getFileResponse = await getFileResponseMessage.Content.ReadFromJsonAsync<GetFileResponse>();

        getFileResponse.Should().NotBeNull();
        getFileResponse!.ResponseTag.Should().Be(GetFileResponse.Tag.UserNotFound);
    }

    [Fact]
    public async Task GetFile_FileNotFound()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        var userId = await CreateUser(httpClient);

        //
        var getFileResponseMessage = await httpClient.GetAsync($"api/files/getFile?userId={userId}&fileId={Guid.NewGuid()}");

        //
        getFileResponseMessage.IsSuccessStatusCode.Should().BeTrue();

        var getFileResponse = await getFileResponseMessage.Content.ReadFromJsonAsync<GetFileResponse>();

        getFileResponse.Should().NotBeNull();
        getFileResponse!.ResponseTag.Should().Be(GetFileResponse.Tag.FileNotFound);
    }

    [Fact]
    public async Task UploadFile_LimitExceededTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        var userId = await CreateUser(httpClient);

        var sourceFile = await File.ReadAllBytesAsync("appsettings.json");

        //
        var tasks = new List<Task<HttpResponseMessage>>();

        for (int i = 0; i < 10; i++)
        {
            var requestContent = new MultipartFormDataContent();

            var fileContent = new ByteArrayContent(sourceFile);
            requestContent.Add(fileContent, "file", $"filename{i}");

            requestContent.Add(new StringContent(userId.ToString()), "userId");

            tasks.Add(httpClient.PostAsync("api/files/uploadUserFile", requestContent));
        }

        //
        var uploadFileResponseMessages = await Task.WhenAll(tasks);
        var tags = new List<UploadFileResponse.Tag>();
        foreach (var uploadFileResponseMessage in uploadFileResponseMessages)
        {
            uploadFileResponseMessage.IsSuccessStatusCode.Should().BeTrue();

            var uploadFileResponse = await uploadFileResponseMessage.Content.ReadFromJsonAsync<UploadFileResponse>();
            uploadFileResponse.Should().NotBeNull();

            tags.Add(uploadFileResponse!.ResponseTag);
        }

        tags.Should().AllSatisfy(tag =>
            tag.Should().BeOneOf(UploadFileResponse.Tag.FilesCountLimitExceeded, UploadFileResponse.Tag.Success));

        tags.Where(t => t == UploadFileResponse.Tag.Success).Should().HaveCount(3);
        tags.Where(t => t == UploadFileResponse.Tag.FilesCountLimitExceeded).Should().HaveCount(7);

        var typedClient = new TypedClient(httpClient);
        var user = await typedClient.GetUserAsync(userId);
        user!.FilesCount.Should().Be(3);
    }

    [Fact]
    public async Task UploadFile_UserNotFoundTest()
    {
        const string filename = "appsettings.pdf";

        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();
        
        var sourceFile = await File.ReadAllBytesAsync("appsettings.json");

        //
        var requestContent = new MultipartFormDataContent();

        var fileContent = new ByteArrayContent(sourceFile);
        requestContent.Add(fileContent, "file", filename);

        requestContent.Add(new StringContent(Guid.NewGuid().ToString()), "userId");

        var uploadFileResponseMessage = await httpClient.PostAsync("api/files/uploadUserFile", requestContent);
        
        //
        uploadFileResponseMessage.IsSuccessStatusCode.Should().BeTrue();

        var uploadFileResponse = await uploadFileResponseMessage.Content.ReadFromJsonAsync<UploadFileResponse>();
        uploadFileResponse.Should().NotBeNull();
        uploadFileResponse!.ResponseTag.Should().Be(UploadFileResponse.Tag.UserNotFound);
    }

    private static async Task<Guid> CreateUser(HttpClient httpClient)
    {
        var typedClient = new TypedClient(httpClient);
        var parentResponse = await typedClient.CreateUserAsync("User1");
        parentResponse.ResponseTag.Should().Be(CreateUserResponse.Tag.UserCreated);
        var parentId = parentResponse.UserId;

        return parentId;
    }
}
