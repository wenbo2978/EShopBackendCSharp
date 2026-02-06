public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(int? categoryId, string? search);
    Task<Product?> GetByIdAsync(int id);

    Task<Product> AddAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Product product);

    Task<bool> CategoryExistsAsync(int categoryId);
}
