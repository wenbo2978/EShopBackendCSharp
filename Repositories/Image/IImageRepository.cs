public interface IImageRepository
{
    Task<List<Image>> GetByProductIdAsync(int productId);
    Task<Image?> GetByIdAsync(int id);

    Task<Image> AddAsync(Image image);
    Task<bool> UpdateAsync(Image image);
    Task<bool> DeleteAsync(Image image);

    Task<bool> ProductExistsAsync(int productId);

    // for "only one primary image" rule
    Task ClearPrimaryForProductAsync(int productId);
}
