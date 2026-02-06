public class CartService : ICartService
{
    private readonly ICartRepository _repo;

    public CartService(ICartRepository repo)
    {
        _repo = repo;
    }

    public async Task<CartDto> GetMyCartAsync(string userId)
    {
        var cart = await EnsureCartAsync(userId);
        return ToDto(cart);
    }

    public async Task<(bool Ok, string? Error, CartDto? Cart)> AddItemAsync(string userId, AddCartItemRequest req)
    {
        if (req.ProductId <= 0) return (false, "ProductId must be > 0.", null);
        if (req.Quantity <= 0) return (false, "Quantity must be > 0.", null);

        var product = await _repo.GetProductAsync(req.ProductId);
        if (product == null) return (false, "Product not found.", null);

        var cart = await EnsureCartAsync(userId);

        // If item already exists in cart -> increase quantity
        var existing = await _repo.GetCartItemByCartAndProductAsync(cart.Id, req.ProductId);
        if (existing != null)
        {
            existing.Quantity += req.Quantity;
            await _repo.SaveChangesAsync();

            // reload full cart (with includes) for response
            var updatedCart = await _repo.GetCartByUserIdAsync(userId) ?? cart;
            return (true, null, ToDto(updatedCart));
        }

        // new item
        cart.Items.Add(new CartItem
        {
            ProductId = req.ProductId,
            Quantity = req.Quantity
        });

        await _repo.SaveChangesAsync();

        var finalCart = await _repo.GetCartByUserIdAsync(userId) ?? cart;
        return (true, null, ToDto(finalCart));
    }

    public async Task<(bool Ok, string? Error, CartDto? Cart)> UpdateItemQuantityAsync(string userId, int cartItemId, int quantity)
    {
        if (quantity <= 0) return (false, "Quantity must be > 0.", null);

        var item = await _repo.GetCartItemAsync(cartItemId);
        if (item == null) return (false, "Cart item not found.", null);

        // Ownership check
        var cart = await EnsureCartAsync(userId);
        if (item.CartId != cart.Id) return (false, "Forbidden: item does not belong to your cart.", null);

        item.Quantity = quantity;
        await _repo.SaveChangesAsync();

        var updatedCart = await _repo.GetCartByUserIdAsync(userId) ?? cart;
        return (true, null, ToDto(updatedCart));
    }

    public async Task<(bool Ok, string? Error)> RemoveItemAsync(string userId, int cartItemId)
    {
        var item = await _repo.GetCartItemAsync(cartItemId);
        if (item == null) return (false, "Cart item not found.");

        var cart = await EnsureCartAsync(userId);
        if (item.CartId != cart.Id) return (false, "Forbidden: item does not belong to your cart.");

        await _repo.DeleteCartItemAsync(item);
        return (true, null);
    }

    private async Task<Cart> EnsureCartAsync(string userId)
    {
        var cart = await _repo.GetCartByUserIdAsync(userId);
        return cart ?? await _repo.CreateCartAsync(userId);
    }

    private static CartDto ToDto(Cart cart)
    {
        var items = cart.Items.Select(ci =>
            new CartItemDto(
                Id: ci.Id,
                CartId: ci.CartId,
                ProductId: ci.ProductId,
                ProductName: ci.Product?.Name ?? "",
                UnitPrice: ci.Product?.Price ?? 0m,
                Quantity: ci.Quantity,
                TotalPrice: ci.TotalPrice
            )).ToList();

        var total = items.Sum(i => i.UnitPrice * i.Quantity);

        return new CartDto(
            Id: cart.Id,
            UserId: cart.UserId,
            Items: items,
            TotalAmount: total
        );
    }
}
