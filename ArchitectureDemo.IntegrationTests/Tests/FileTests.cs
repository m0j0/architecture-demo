using System.Net;
using System.Net.Http.Json;
using ArchitectureDemo.gRPC.Services;
using ArchitectureDemo.WebApi.Host.Dtos;
using FluentAssertions;
using Xunit.Abstractions;

namespace ArchitectureDemo.IntegrationTests.Tests;

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
    public async Task UploadFile_Scenario()
    {
        const string filename = "appsettings.pdf";

        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        using var gRpcChannel = factory.CreateGRpcChannel();
        var usersClient = new Users.UsersClient(gRpcChannel);
        var httpClient = factory.CreateClient();

        var userId = await CreateUser(usersClient);

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

        var fileId = uploadFileResponse!.FileId;

        //
        var downloadedFileResponse = await httpClient.GetAsync($"api/files/getFile?userId={userId}&fileId={fileId}");

        downloadedFileResponse.IsSuccessStatusCode.Should().BeTrue();

        var downloadedFile = await downloadedFileResponse.Content.ReadAsByteArrayAsync();

        downloadedFile.Should().BeEquivalentTo(sourceFile);
    }

    [Fact]
    public async Task GetFile_UserNotFoundTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        //
        var getFileResponseMessage = await httpClient.GetAsync($"api/files/getFile?userId={100100}&fileId={200200}");

        //
        getFileResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var getFileResponse = await getFileResponseMessage.Content.ReadFromJsonAsync<GetFileErrorResponse>();

        getFileResponse.Should().NotBeNull();
        getFileResponse!.Code.Should().Be(GetFileErrorCode.UserNotFound);
    }

    [Fact]
    public async Task GetFile_FileNotFound()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        using var gRpcChannel = factory.CreateGRpcChannel();
        var usersClient = new Users.UsersClient(gRpcChannel);
        var httpClient = factory.CreateClient();

        var userId = await CreateUser(usersClient);

        //
        var getFileResponseMessage = await httpClient.GetAsync($"api/files/getFile?userId={userId}&fileId={100200}");

        //
        getFileResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var getFileResponse = await getFileResponseMessage.Content.ReadFromJsonAsync<GetFileErrorResponse>();

        getFileResponse.Should().NotBeNull();
        getFileResponse!.Code.Should().Be(GetFileErrorCode.FileNotFound);
    }

    [Fact]
    public async Task UploadFile_LimitExceededTest()
    {
        await using var factory = await _context.CreateWebApplicationFactory(_testOutputHelper);
        var httpClient = factory.CreateClient();

        using var gRpcChannel = factory.CreateGRpcChannel();
        var usersClient = new Users.UsersClient(gRpcChannel);

        var userId = await CreateUser(usersClient);

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

        int successCount = 0;
        var errorCodes = new List<UploadFileErrorCode>();
        foreach (var uploadFileResponseMessage in uploadFileResponseMessages)
        {
            if (uploadFileResponseMessage.IsSuccessStatusCode)
            {
                successCount++;
                continue;
            }

            uploadFileResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var uploadFileErrorResponse = await uploadFileResponseMessage.Content.ReadFromJsonAsync<UploadFileErrorResponse>();
            uploadFileErrorResponse.Should().NotBeNull();

            errorCodes.Add(uploadFileErrorResponse!.Code);
        }

        errorCodes.Should().AllSatisfy(tag =>
            tag.Should().BeOneOf(
                UploadFileErrorCode.AlreadyLocked,
                UploadFileErrorCode.FilesCountLimitExceeded));

        successCount.Should().BeGreaterOrEqualTo(1).And.BeLessOrEqualTo(3);
        errorCodes.Where(t => t == UploadFileErrorCode.AlreadyLocked).Should().HaveCountLessOrEqualTo(9);
        errorCodes.Where(t => t == UploadFileErrorCode.FilesCountLimitExceeded).Should().HaveCountLessOrEqualTo(7);
        
        var userResponse = await usersClient.GetByIdAsync(new GetByIdRequest { Id = userId });
        userResponse.User!.FilesCount.Should().Be(successCount);
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

        requestContent.Add(new StringContent("200000"), "userId");

        var uploadFileResponseMessage = await httpClient.PostAsync("api/files/uploadUserFile", requestContent);

        //
        uploadFileResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var uploadFileResponse = await uploadFileResponseMessage.Content.ReadFromJsonAsync<UploadFileErrorResponse>();
        uploadFileResponse.Should().NotBeNull();
        uploadFileResponse!.Code.Should().Be(UploadFileErrorCode.UserNotFound);
    }

    private static async Task<int> CreateUser(Users.UsersClient usersClient)
    {
        var createResponse = await usersClient.CreateAsync(
            new CreateRequest { Name = "User1", Email = "a@b" });
        createResponse.ResultCase.Should().Be(CreateResponse.ResultOneofCase.UserId);
        return createResponse.UserId;
    }
}
