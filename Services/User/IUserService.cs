public interface IUserService
{
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto?> GetByEmailAsync(string email);

    Task<(bool Ok, IEnumerable<string> Errors, UserDto? User)> UpdateAsync(string id, UpdateUserRequest req);

    Task<(bool Ok, IEnumerable<string> Errors)> DeleteAsync(string id);
}