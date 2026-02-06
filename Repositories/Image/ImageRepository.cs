using Microsoft.EntityFrameworkCore;

public class ImageRepository : IImageRepository
{
    private readonly AppDbContext _db;

    public ImageRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Image>> GetByProductIdAsync(int productId)
        => _db.Images.AsNoTracking()
            .Where(i => i.ProductId == productId)
            .OrderByDescending(i => i.IsPrimary)
            .ToListAsync();

    public Task<Image?> GetByIdAsync(int id)
        => _db.Images.FirstOrDefaultAsync(i => i.Id == id);

    public async Task<Image> AddAsync(Image image)
    {
        _db.Images.Add(image);
        await _db.SaveChangesAsync();
        return image;
    }

    public async Task<bool> UpdateAsync(Image image)
    {
        await _db.SaveChangesAsync(); // assumes tracked
        return true;
    }

    public async Task<bool> DeleteAsync(Image image)
    {
        _db.Images.Remove(image);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<bool> ProductExistsAsync(int productId)
        => _db.Products.AnyAsync(p => p.Id == productId);

    public async Task ClearPrimaryForProductAsync(int productId)
    {
        // set IsPrimary = false for all images of that product
        var imgs = await _db.Images.Where(i => i.ProductId == productId && i.IsPrimary).ToListAsync();
        foreach (var img in imgs)
            img.IsPrimary = false;

        await _db.SaveChangesAsync();
    }
}
