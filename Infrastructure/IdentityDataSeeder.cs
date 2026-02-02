using Microsoft.AspNetCore.Identity;

public class IdentityDataSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;

    public IdentityDataSeeder(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }

    public async Task SeedAsync()
    {
        await CreateRoleIfMissing("Admin");
        await CreateRoleIfMissing("User");

        var adminEmail = _config["Seed:AdminEmail"] ?? "admin@eshop.com";
        var adminPassword = _config["Seed:AdminPassword"] ?? "Admin123!";

        var admin = await _userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Admin"
            };

            // ✅ hashes password into PasswordHash
            var create = await _userManager.CreateAsync(admin, adminPassword);
            if (!create.Succeeded)
                throw new Exception(string.Join("; ", create.Errors.Select(e => e.Description)));
        }

        if (!await _userManager.IsInRoleAsync(admin, "Admin"))
            await _userManager.AddToRoleAsync(admin, "Admin");
    }

    private async Task CreateRoleIfMissing(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var r = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!r.Succeeded)
                throw new Exception(string.Join("; ", r.Errors.Select(e => e.Description)));
        }
    }
}
