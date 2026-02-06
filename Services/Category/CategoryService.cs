public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _repo.GetAllAsync();
        return categories.Select(ToDto).ToList();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        return category == null ? null : ToDto(category);
    }

    public async Task<(bool Ok, string? Error, CategoryDto? Data)> CreateAsync(CreateCategoryRequest req)
    {
        // basic validation
        if (string.IsNullOrWhiteSpace(req.Name))
            return (false, "Name is required.", null);

        var category = new Category { Name = req.Name.Trim() };
        var saved = await _repo.AddAsync(category);

        return (true, null, ToDto(saved));
    }

    public async Task<(bool Ok, string? Error, CategoryDto? Data)> UpdateAsync(int id, UpdateCategoryRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            return (false, "Name is required.", null);

        var category = await _repo.GetByIdAsync(id);
        if (category == null)
            return (false, "Category not found.", null);

        category.Name = req.Name.Trim();
        await _repo.UpdateAsync(category);

        return (true, null, ToDto(category));
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null)
            return (false, "Category not found.");

        await _repo.DeleteAsync(category);
        return (true, null);
    }

    private static CategoryDto ToDto(Category c) => new(c.Id, c.Name);
}
