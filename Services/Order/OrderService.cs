public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;

    public OrderService(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<OrderDto>> GetMyOrdersAsync(string userId)
    {
        var orders = await _repo.GetOrdersByUserIdAsync(userId);
        return orders.Select(ToDto).ToList();
    }

    public async Task<(bool Ok, string? Error, OrderDto? Data)> GetMyOrderByIdAsync(string userId, int orderId)
    {
        var order = await _repo.GetOrderByIdAsync(orderId);
        if (order == null) return (false, "Order not found.", null);

        // ownership check
        if (order.UserId != userId) return (false, "Forbidden.", null);

        return (true, null, ToDto(order));
    }

    public async Task<(bool Ok, string? Error, OrderDto? Data)> CheckoutAsync(string userId)
    {
        var cart = await _repo.GetCartByUserIdAsync(userId);
        if (cart == null) return (false, "Cart not found.", null);
        if (cart.Items.Count == 0) return (false, "Cart is empty.", null);

        // Build order from cart items
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
        };

        foreach (var ci in cart.Items)
        {
            if (ci.Product == null)
                return (false, "Cart contains invalid product.", null);

            if (ci.Quantity <= 0)
                return (false, "Cart contains invalid quantity.", null);

            order.OrderItems.Add(new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity
            });
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.Product.Price * i.Quantity);

        // Save order first
        var saved = await _repo.AddOrderAsync(order);

        // Then clear cart
        await _repo.ClearCartItemsAsync(cart.Id);

        // Reload with product includes for response (optional)
        var reloaded = await _repo.GetOrderByIdAsync(saved.Id) ?? saved;

        return (true, null, ToDto(reloaded));
    }

    private static OrderDto ToDto(Order o)
    {
        var items = o.OrderItems.Select(oi =>
            new OrderItemDto(
                Id: oi.Id,
                ProductId: oi.ProductId,
                UnitPrice: oi.Product.Price,
                ProductName: oi.Product?.Name ?? "",
                Quantity: oi.Quantity
            )).ToList();

        var total = items.Sum(x => x.UnitPrice * x.Quantity);

        return new OrderDto(
            Id: o.Id,
            OrderDate: o.OrderDate,
            OrderStatus: o.OrderStatus,
            TotalAmount: total,
            Items: items
        );
    }
}
