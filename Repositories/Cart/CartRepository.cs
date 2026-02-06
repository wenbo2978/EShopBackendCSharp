using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _db;

    public CartRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Cart?> GetCartByUserIdAsync(string userId)
        => _db.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

    public async Task<Cart> CreateCartAsync(string userId)
    {
        var cart = new Cart { UserId = userId };
        _db.Carts.Add(cart);
        await _db.SaveChangesAsync();
        return cart;
    }

    public Task<Product?> GetProductAsync(int productId)
        => _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);

    public Task<CartItem?> GetCartItemAsync(int cartItemId)
        => _db.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

    public Task<CartItem?> GetCartItemByCartAndProductAsync(int cartId, int productId)
        => _db.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();

    public async Task DeleteCartItemAsync(CartItem item)
    {
        _db.CartItems.Remove(item);
        await _db.SaveChangesAsync();
    }
}
