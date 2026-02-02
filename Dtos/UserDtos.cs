public record UserDto(
    string Id,
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    IList<string> Roles
);

public record UpdateUserRequest(
    string FirstName,
    string LastName
);

public class CreateUserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName {get; set; } = null!;
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}