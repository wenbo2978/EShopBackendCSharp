public class Role
{
    public int Id { set; get; }
    public required string Name { set; get; }

    public List<User> Users { get; set; } = new();

}