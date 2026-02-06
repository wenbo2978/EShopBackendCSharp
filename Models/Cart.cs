public class Cart
{
    public int Id { set; get; }
    public decimal TotalAmount { set; get; } = 0;
    public string UserId { set; get; } = null!;
    public AppUser? User { set; get; }
    public List<CartItem> Items { set; get; } = new();
}