using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;

    public CategoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Category>> GetAllAsync()
        => _db.Categories.AsNoTracking().ToListAsync();

    public Task<Category?> GetByIdAsync(int id)
        => _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Category> AddAsync(Category category)
    {
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return category;
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        // category is tracked if fetched via GetByIdAsync above
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Category category)
    {
        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        return true;
    }
}
