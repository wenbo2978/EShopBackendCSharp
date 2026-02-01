public class CartItem
{
    public int Id { set; get; }
    public int CartId { set; get; }
    public int ProductId { set; get; }
    public Product? Product { set; get; }
    public decimal UnitPrice { set; get; } = 0;
    public decimal TotalPrice { set; get; } = 0;
    public int Quantity { set; get; } = 0;
}