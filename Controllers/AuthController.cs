using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtTokenService _jwt;

    public AuthController(UserManager<AppUser> userManager, JwtTokenService jwt)
    {
        _userManager = userManager;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserRequest req)
    {
        // 1) check email duplication
        var existing = await _userManager.FindByEmailAsync(req.Email);
        if (existing != null)
            return BadRequest("Email already exists.");

        var user = new AppUser
        {
            UserName = req.Email,
            Email = req.Email,
            FirstName = req.FirstName,
            LastName = req.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, req.Password);

        // 3) assign default role
        const string defaultRole = "User";

        // If you didn't seed roles yet, this will fail unless the role exists
        var addRole = await _userManager.AddToRoleAsync(user, defaultRole);

        if (!addRole.Succeeded)
            return BadRequest(addRole.Errors);

        return Ok("User created.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        // 1) find user
        var user = await _userManager.FindByEmailAsync(req.Email);
        if (user == null)
            return Unauthorized("Invalid email or password.");

        // 2) verify password (Identity checks PasswordHash)
        var ok = await _userManager.CheckPasswordAsync(user, req.Password);
        if (!ok)
            return Unauthorized("Invalid email or password.");

        // 3) roles -> claims
        var roles = await _userManager.GetRolesAsync(user);

        // 4) generate JWT
        var token= _jwt.CreateToken(user, roles);

        return Ok(new
        {
            accessToken = token
        });
    }


}