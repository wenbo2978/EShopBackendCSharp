public class CreateUserDto
{
    public required string FirstName { set; get; }
    public required string LastName { set; get; }
    public required string Email { set; get; }
    public required string Password { set; get; }
    
    public required int RoleId { set; get; }

    public override string ToString()
    {
        return "User: {FirstName = " + FirstName + ", LastName = " + LastName +
            ", Email = " + Email + ", Password = " + Password + ", RoleId = " + RoleId + "}";
    }
}