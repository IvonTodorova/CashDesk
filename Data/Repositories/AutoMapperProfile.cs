using AutoMapper;
using CashDesk.Data.Dto;
using CashDesk.Data.Models;

namespace CashDesk.Data.Repositories
{ 
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //ignore propeterty 
            CreateMap<User, UserDto>()
                .ForMember(dst => dst.AutificationKey, opt => opt.Ignore());
        }
    }
}