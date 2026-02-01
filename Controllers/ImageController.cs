using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/image")]
public class ImageController : ControllerBase
{
    private readonly S3Uploader _s3Uploader;
    private string? accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")
        ?? throw new InvalidOperationException("AWS_ACCESS_KEY Is Missing!");
    private string? secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY")
        ?? throw new InvalidOperationException("AWS_SECRET_KEY Is Missing!");
    private string? region = Environment.GetEnvironmentVariable("AWS_REGION")
        ?? throw new InvalidOperationException("AWS_REGION Is Missing!");
    private string? bucketName = Environment.GetEnvironmentVariable("AWS_BUCKETNAME");
    private string? folderName = Environment.GetEnvironmentVariable("AWS_FOLDER");

    public ImageController()
    {
        _s3Uploader = new S3Uploader(accessKey, secretKey, region);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> UploadImage(IFormFileCollection files, int id)
    {
        Console.WriteLine($"Product Id = {id}");
        if (files == null || files.Count == 0)
        {
            return BadRequest("No files uploaded.");
        }
        var uploadedUrls = new List<string>();
        if (bucketName == null)
            throw new InvalidOperationException("AWS_BUCKETNAME Setting is Invalid!");
        if(folderName == null)
            throw new InvalidOperationException("AWS_FOLDER Setting is Invalid!");
        //string bucketName = "eshop-bucket-chen1997";
            foreach (var file in files)
            {
                string keyName = $"{folderName}/{Guid.NewGuid()}_{file.FileName}";
                var url = await _s3Uploader.UploadFileAsync(file, bucketName, keyName);
                string key = new Uri(url).AbsolutePath.TrimStart('/');
                string pic = key.Split('/')[1];
                uploadedUrls.Add(pic);
            }


        return Ok(new { urls = uploadedUrls });
    }

    [HttpDelete("{id}/{fileName}")]
    public async Task<IActionResult> DeleteImage(int id, string fileName)
    {
        if (bucketName == null)
            throw new InvalidOperationException("AWS_BUCKETNAME Setting is Invalid!");
        if(folderName == null)
            throw new InvalidOperationException("AWS_FOLDER Setting is Invalid!");
        string keyName = $"{folderName}/{fileName}";
        await _s3Uploader.DeleteFileAsync(bucketName, keyName);
        return Ok(new { message = "File deleted successfully." });
    }

}