public interface IUserService
{
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto> CreateUser(CreateUserDto userDto);

    Task<UserDto?> GetUser(int id);
    Task<bool> UpdateUser(int id, UpdateUserDto userDto);
    Task<bool> DeleteUser(int id);
}