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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _service.GetUser(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        //Console.WriteLine(userDto.ToString());
        var createdUser = await _service.CreateUser(userDto);

        //return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        return Ok(createdUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditUser(int id, [FromBody] UpdateUserDto userDto)
    {
        var success = await _service.UpdateUser(id, userDto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _service.DeleteUser(id);
        return success ? NoContent() : NotFound();
    }
}