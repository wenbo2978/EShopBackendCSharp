public class User
{
    public int Id { set; get; }
    public required string FirstName { set; get; }
    public required string LastName { set; get; }
    public required string Email { set; get; }
    public required string Password { set; get; }

    public int RoleId { get; set; }

    public Role? Role { get; set; }

    public Cart? Cart { set; get; }
}