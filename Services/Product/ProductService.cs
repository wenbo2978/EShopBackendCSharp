public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ProductDto>> GetAllAsync(int? categoryId, string? search)
    {
        var products = await _repo.GetAllAsync(categoryId, search);
        return products.Select(ToDto).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        return product == null ? null : ToDto(product);
    }

    public async Task<(bool Ok, string? Error, ProductDto? Data)> CreateAsync(CreateProductRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            return (false, "Name is required.", null);

        if (req.Price < 0)
            return (false, "Price must be >= 0.", null);

        if (!await _repo.CategoryExistsAsync(req.CategoryId))
            return (false, "Category does not exist.", null);

        var product = new Product
        {
            Name = req.Name.Trim(),
            Price = req.Price,
            Brand = req.Brand,
            Inventory = req.Inventory,
            Description = req.Description,
            CategoryId = req.CategoryId
        };

        var saved = await _repo.AddAsync(product);
        return (true, null, ToDto(saved));
    }

    public async Task<(bool Ok, string? Error, ProductDto? Data)> UpdateAsync(int id, UpdateProductRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            return (false, "Name is required.", null);

        if (req.Price < 0)
            return (false, "Price must be >= 0.", null);

        if (!await _repo.CategoryExistsAsync(req.CategoryId))
            return (false, "Category does not exist.", null);

        var product = await _repo.GetByIdAsync(id);
        if (product == null)
            return (false, "Product not found.", null);

        product.Name = req.Name.Trim();
        product.Price = req.Price;
        product.Brand = req.Brand;
        product.Inventory =req.Inventory;
        product.Description = req.Description;
        product.CategoryId = req.CategoryId;

        await _repo.UpdateAsync(product);
        return (true, null, ToDto(product));
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product == null)
            return (false, "Product not found.");

        await _repo.DeleteAsync(product);
        return (true, null);
    }

    private static ProductDto ToDto(Product p)
        => new(p.Id, p.Name, p.Brand, p.Price, 
        p.Inventory, new CategoryDto(p.CategoryId, p?.Category?.Name ?? "new Brand"), p?.Images ?? new List<Image?>());
}
