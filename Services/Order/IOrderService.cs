public interface IOrderService
{
    Task<List<OrderDto>> GetMyOrdersAsync(string userId);
    Task<(bool Ok, string? Error, OrderDto? Data)> GetMyOrderByIdAsync(string userId, int orderId);

    Task<(bool Ok, string? Error, OrderDto? Data)> CheckoutAsync(string userId);
}
