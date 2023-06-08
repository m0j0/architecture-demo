using Amazon.S3;
using Amazon.S3.Model;
using ArchitectureDemo.Services;
using System.Net;

namespace ArchitectureDemo.S3.Services;

internal class S3Service : IS3Service
{
    private const string BucketName = "architecture-demo-bucket";

    private readonly IAmazonS3 _amazonS3;

    public S3Service(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public async Task<Stream> GetFile(string fileName,
        CancellationToken cancellationToken)
    {
        var getObjectResponse = await _amazonS3.GetObjectAsync(
            new GetObjectRequest
            {
                BucketName = BucketName,
                Key = fileName
            }, cancellationToken);

        if (getObjectResponse.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException();
        }

        return getObjectResponse.ResponseStream;
    }

    public async Task UploadFile(Stream file, string fileName,
        CancellationToken cancellationToken)
    {
        var putObjectResponse = await _amazonS3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = BucketName,
            Key = fileName,
            AutoCloseStream = false,
            InputStream = file
        }, cancellationToken);

        if (putObjectResponse.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException();
        }
    }
}
