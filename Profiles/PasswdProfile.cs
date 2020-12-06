
using AutoMapper;
using inz_int.DTOs;
using inz_int.Models;

namespace inz_int
{
    public class PasswdProfile : Profile
    {
        public PasswdProfile()
        {
            CreateMap<Password, PasswdReadDTO>();
            CreateMap<PasswordCreateDTO, Password>();
        }
    }
}