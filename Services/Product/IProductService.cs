public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync(int? categoryId, string? search);
    Task<ProductDto?> GetByIdAsync(int id);

    Task<(bool Ok, string? Error, ProductDto? Data)> CreateAsync(CreateProductRequest req);
    Task<(bool Ok, string? Error, ProductDto? Data)> UpdateAsync(int id, UpdateProductRequest req);
    Task<(bool Ok, string? Error)> DeleteAsync(int id);
}
