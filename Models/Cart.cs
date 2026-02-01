public class Cart
{
    public int Id { set; get; }
    public decimal TotalAmount { set; get; } = 0;
    public int UserId { set; get; }
    public User? User { set; get; }
    public List<CartItem?> Items { set; get; } = new();
}