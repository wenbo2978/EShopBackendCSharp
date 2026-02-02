using Microsoft.AspNetCore.Identity;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<AppUser?> FindByIdAsync(string id)
        => _userManager.FindByIdAsync(id);

    public Task<AppUser?> FindByEmailAsync(string email)
        => _userManager.FindByEmailAsync(email);

    public Task<IList<string>> GetRolesAsync(AppUser user)
        => _userManager.GetRolesAsync(user);

    public Task<IdentityResult> UpdateAsync(AppUser user)
        => _userManager.UpdateAsync(user);

    public Task<IdentityResult> DeleteAsync(AppUser user)
        => _userManager.DeleteAsync(user);
}