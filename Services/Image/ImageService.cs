public class ImageService : IImageService
{
    private readonly IImageRepository _repo;

    public ImageService(IImageRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ImageDto>> GetByProductIdAsync(int productId)
    {
        var images = await _repo.GetByProductIdAsync(productId);
        return images.Select(ToDto).ToList();
    }

    public async Task<ImageDto?> GetByIdAsync(int id)
    {
        var image = await _repo.GetByIdAsync(id);
        return image == null ? null : ToDto(image);
    }

    public async Task<(bool Ok, string? Error, ImageDto? Data)> CreateAsync(CreateImageRequest req)
    {
        if (req.ProductId <= 0)
            return (false, "ProductId must be > 0.", null);

        if (string.IsNullOrWhiteSpace(req.DownloadUrl))
            return (false, "Url is required.", null);

        if (string.IsNullOrWhiteSpace(req.FileName))
            return (false, "Key is required.", null);

        var productExists = await _repo.ProductExistsAsync(req.ProductId);
        if (!productExists)
            return (false, "Product does not exist.", null);

        // If you enforce only one primary image:
        if (req.IsPrimary)
            await _repo.ClearPrimaryForProductAsync(req.ProductId);

        var image = new Image
        {
            ProductId = req.ProductId,
            DownloadUrl = req.DownloadUrl.Trim(),
            FileName = req.FileName.Trim(),
            FileType = req.FileType.Trim(),
            IsPrimary = req.IsPrimary,
        };

        var saved = await _repo.AddAsync(image);
        return (true, null, ToDto(saved));
    }

    public async Task<(bool Ok, string? Error, ImageDto? Data)> UpdateAsync(int id, UpdateImageRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.DownloadUrl))
            return (false, "Url is required.", null);

        if (string.IsNullOrWhiteSpace(req.FileName))
            return (false, "Key is required.", null);

        var image = await _repo.GetByIdAsync(id);
        if (image == null)
            return (false, "Image not found.", null);

        if (req.IsPrimary)
            await _repo.ClearPrimaryForProductAsync(image.ProductId);

        image.DownloadUrl = req.DownloadUrl.Trim();
        image.FileName = req.FileName.Trim();
        image.FileType = req.FileType.Trim();
        image.IsPrimary = req.IsPrimary;

        await _repo.UpdateAsync(image);
        return (true, null, ToDto(image));
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id)
    {
        var image = await _repo.GetByIdAsync(id);
        if (image == null)
            return (false, "Image not found.");

        await _repo.DeleteAsync(image);
        return (true, null);
    }

    private static ImageDto ToDto(Image i)
        => new(i.Id, i.FileName, i.FileType, i.DownloadUrl, i.IsPrimary, i.ProductId);
}
