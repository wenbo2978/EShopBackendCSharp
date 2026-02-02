using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    // GET /api/users/{id}
    // Typical: only Admin can query anyone
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user == null) return NotFound("User not found.");

        return Ok(user);
    }

    // GET /api/users/by-email?email=xx
    [Authorize(Roles = "Admin")]
    [HttpGet("by-email")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        var user = await _service.GetByEmailAsync(email);
        if (user == null) return NotFound("User not found.");

        return Ok(user);
    }

    // PUT /api/users/{id}
    // Here: Admin can update. (If you want self-update, I can show that too.)
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserRequest req)
    {
        var (ok, errors, user) = await _service.UpdateAsync(id, req);
        if (!ok)
        {
            // NotFound vs BadRequest: decide by error message
            if (errors.Contains("User not found."))
                return NotFound("User not found.");

            return BadRequest(errors);
        }

        return Ok(user);
    }

    // DELETE /api/users/{id}
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var (ok, errors) = await _service.DeleteAsync(id);
        if (!ok)
        {
            if (errors.Contains("User not found."))
                return NotFound("User not found.");

            return BadRequest(errors);
        }

        return NoContent();
    }
}