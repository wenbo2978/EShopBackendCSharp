public record CartItemDto(
    int Id,
    int CartId,
    int ProductId,
    string ProductName,
    decimal UnitPrice,
    decimal TotalPrice,
    int Quantity
);

public record CartDto(
    int Id,
    string UserId,
    decimal TotalAmount,
    List<CartItemDto> Items
);

public record AddCartItemRequest(int ProductId, int Quantity);
public record UpdateCartItemQuantityRequest(int Quantity);
