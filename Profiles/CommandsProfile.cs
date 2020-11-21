using AutoMapper;
using inz_int.DTOs;
using inz_int.Models;

namespace inz_int.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserReadDTO>();
        }
    }
}
