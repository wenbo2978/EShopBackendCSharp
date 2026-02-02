using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<UserDto, AppUser>();
        CreateMap<CreateUserDto, AppUser>();
        CreateMap<AppUser, CreateUserDto>();
        CreateMap<UpdateUserDto, AppUser>();
        CreateMap<AppUser, UpdateUserDto>();
    }
}