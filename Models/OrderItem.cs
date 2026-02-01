public class OrderItem
{
    public int Id { set; get; }
    public int Quantity { set; get; }
    public int OrderId { set; get; }
    public int ProductId { set; get; }
    public Product? Product { set; get; }
}