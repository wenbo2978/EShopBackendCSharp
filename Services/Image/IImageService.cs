public interface IImageService
{
    Task<List<ImageDto>> GetByProductIdAsync(int productId);
    Task<ImageDto?> GetByIdAsync(int id);

    Task<(bool Ok, string? Error, ImageDto? Data)> CreateAsync(CreateImageRequest req);
    Task<(bool Ok, string? Error, ImageDto? Data)> UpdateAsync(int id, UpdateImageRequest req);
    Task<(bool Ok, string? Error)> DeleteAsync(int id);
}
