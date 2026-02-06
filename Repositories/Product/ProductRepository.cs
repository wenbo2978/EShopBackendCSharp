using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetAllAsync(int? categoryId, string? search)
    {
        IQueryable<Product> query = _db.Products
            .Include(p => p.Category)
            .AsNoTracking();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(p => p.Name.Contains(s));
        }

        return await query.ToListAsync();
    }

    public Task<Product?> GetByIdAsync(int id)
        => _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> AddAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        // ensure Category navigation is populated for response
        await _db.Entry(product).Reference(p => p.Category).LoadAsync();
        return product;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        // product is tracked if loaded by GetByIdAsync
        await _db.Entry(product).Reference(p => p.Category).LoadAsync();
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Product product)
    {
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<bool> CategoryExistsAsync(int categoryId)
        => _db.Categories.AnyAsync(c => c.Id == categoryId);
}
