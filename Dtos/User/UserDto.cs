public class UserDto
{
    public int Id { set; get; }
    public required string FirstName { set; get; }
    public required string LastName { set; get; }
    public required string Email { set; get; }
    public required int RoleId { set; get; }
    public RoleDto? Role { set; get; }
}