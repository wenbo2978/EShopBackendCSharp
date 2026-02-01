public class CreateProductDto
{
    public int Id { set; get; }
    public required string Name { set; get; }
    public required string Brand { set; get; }
    public required decimal Price { set; get; }
    public required int Inventory { set; get; }
    public required string Description { set; get; }
    public int CategoryId { set; get; }
}