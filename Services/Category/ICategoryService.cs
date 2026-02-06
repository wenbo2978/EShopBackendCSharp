public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);

    Task<(bool Ok, string? Error, CategoryDto? Data)> CreateAsync(CreateCategoryRequest req);
    Task<(bool Ok, string? Error, CategoryDto? Data)> UpdateAsync(int id, UpdateCategoryRequest req);
    Task<(bool Ok, string? Error)> DeleteAsync(int id);
}
