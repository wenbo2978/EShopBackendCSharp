using Microsoft.AspNetCore.Identity;

public interface IUserRepository
{
    Task<AppUser?> FindByIdAsync(string id);
    Task<AppUser?> FindByEmailAsync(string email);
    Task<IList<string>> GetRolesAsync(AppUser user);

    Task<IdentityResult> UpdateAsync(AppUser user);
    Task<IdentityResult> DeleteAsync(AppUser user);
}