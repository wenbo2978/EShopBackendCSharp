public record ProductDto(
    int Id,
    string Name,
    string Brand,
    decimal Price,
    int Inventory,
    CategoryDto Category,
    List<Image?> Images
);

public record CreateProductRequest(
    string Name,
    string Brand,
    decimal Price,
    int Inventory,
    string Description,
    int CategoryId
);

public record UpdateProductRequest(
    string Name,
    string Brand,
    decimal Price,
    int Inventory,
    string Description,
    int CategoryId
);

