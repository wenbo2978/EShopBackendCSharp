public interface ICartService
{
    Task<CartDto> GetMyCartAsync(string userId);

    Task<(bool Ok, string? Error, CartDto? Cart)> AddItemAsync(string userId, AddCartItemRequest req);
    Task<(bool Ok, string? Error, CartDto? Cart)> UpdateItemQuantityAsync(string userId, int cartItemId, int quantity);
    Task<(bool Ok, string? Error)> RemoveItemAsync(string userId, int cartItemId);
}
