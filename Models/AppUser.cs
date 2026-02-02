using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public required string FirstName { set; get; }
    public required string LastName { set; get; }

    public Cart? Cart { set; get; }
}