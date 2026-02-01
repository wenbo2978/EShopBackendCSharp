using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

public class S3Uploader
{
    private readonly IAmazonS3 _s3Client;

    public S3Uploader(string accessKey, string secretKey, string region)
    {
        _s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
    }

    public async Task<string> UploadFileAsync(IFormFile file, string bucketName, string keyName)
    {
        using var stream = file.OpenReadStream();
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            Key = keyName,
            BucketName = bucketName,
            ContentType = file.ContentType
        };
        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(uploadRequest);
        return $"https://{bucketName}.s3.amazonaws.com/{keyName}";
    }

    public async Task DeleteFileAsync(string bucketName, string keyName)
    {
        var deleteRequest = new Amazon.S3.Model.DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = keyName
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }


}