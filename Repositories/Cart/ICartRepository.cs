public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<Cart> CreateCartAsync(string userId);

    Task<Product?> GetProductAsync(int productId);

    Task<CartItem?> GetCartItemAsync(int cartItemId);
    Task<CartItem?> GetCartItemByCartAndProductAsync(int cartId, int productId);

    Task SaveChangesAsync();
    Task DeleteCartItemAsync(CartItem item);
}
