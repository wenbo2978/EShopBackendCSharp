
using AutoMapper;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public Task<List<UserDto>> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public async Task<UserDto> CreateUser(CreateUserDto userDto)
    {
        // Console.WriteLine("---------------------------Service!!!");
        // Console.WriteLine(userDto.ToString());
        var user = _mapper.Map<User>(userDto);
        var created = await _repo.CreateAsync(user);
        return _mapper.Map<UserDto>(created);
    }

    public async Task<UserDto?> GetUser(int id)
    {
        var user = await _repo.GetUserAsync(id);
        //Console.WriteLine(user?.Role?.Name);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<bool> UpdateUser(int id, UpdateUserDto userDto)
    {
        var user = await _repo.GetUserAsync(id);
        if (user == null) return false;

        _mapper.Map(userDto, user);
        await _repo.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _repo.GetUserAsync(id);
        if (user == null) return false;

        await _repo.DeleteAsync(user);
        return true;
    }
}