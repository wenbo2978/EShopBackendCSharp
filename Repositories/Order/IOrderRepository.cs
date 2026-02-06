public interface IOrderRepository
{
    Task<List<Order>> GetOrdersByUserIdAsync(string userId);
    Task<Order?> GetOrderByIdAsync(int orderId);

    Task<Cart?> GetCartByUserIdAsync(string userId);

    Task<Order> AddOrderAsync(Order order);
    Task SaveChangesAsync();

    Task ClearCartItemsAsync(int cartId);
}
