public class Order
{
    public int Id { set; get; }
    public DateTime OrderDate { set; get; }
    public decimal TotalAmount { set; get; }
    public OrderStatus OrderStatus { set; get; }
    public List<OrderItem> OrderItems { set; get; } = new();
    public string UserId { set; get; } = null!;
}