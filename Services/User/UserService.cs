
using AutoMapper;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _repo.FindByIdAsync(id);
        if (user == null) return null;

        return await ToDtoAsync(user);
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _repo.FindByEmailAsync(email);
        if (user == null) return null;

        return await ToDtoAsync(user);
    }

    public async Task<(bool Ok, IEnumerable<string> Errors, UserDto? User)> UpdateAsync(string id, UpdateUserRequest req)
    {
        var user = await _repo.FindByIdAsync(id);
        if (user == null)
            return (false, new[] { "User not found." }, null);

        // Update only allowed fields (don’t touch password here)
        user.FirstName = req.FirstName;
        user.LastName = req.LastName;

        var result = await _repo.UpdateAsync(user);
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description), null);

        var dto = await ToDtoAsync(user);
        return (true, Array.Empty<string>(), dto);
    }

    public async Task<(bool Ok, IEnumerable<string> Errors)> DeleteAsync(string id)
    {
        var user = await _repo.FindByIdAsync(id);
        if (user == null)
            return (false, new[] { "User not found." });

        var result = await _repo.DeleteAsync(user);
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description));

        return (true, Array.Empty<string>());
    }

    private async Task<UserDto> ToDtoAsync(AppUser user)
    {
        var roles = await _repo.GetRolesAsync(user);

        return new UserDto(
            Id: user.Id,
            Email: user.Email ?? "",
            UserName: user.UserName ?? "",
            FirstName: user.FirstName,
            LastName: user.LastName,
            Roles: roles
        );
    }
}