using Auth;
using AutoMapper;
namespace WebApplication2.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //Source -> Target
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>().ForMember(x=>x.Password, opt => opt.Ignore());
            CreateMap<UserUpdateDto, User>();
            CreateMap<User,UserUpdateDto>();
        }
    }
}