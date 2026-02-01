public interface IUserRepository
{
    Task<User> CreateAsync(User user);

    Task<User?> GetUserAsync(int id);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);
}