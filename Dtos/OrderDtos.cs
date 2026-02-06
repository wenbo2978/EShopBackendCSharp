public record OrderItemDto(
    int Id,
    int ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity
);

public record OrderDto(
    int Id,
    DateTime OrderDate,
    decimal TotalAmount,
    OrderStatus OrderStatus,
    List<OrderItemDto> Items
);

public record CheckoutRequest(string? Note = null);

