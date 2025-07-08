using AutoMapper;
using DevLifePortal.Application.DTOs;
using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDTO, User>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();

            CreateMap<DevDatingProfile, DevDatingAddProfileDTO>().ReverseMap();
            CreateMap<DevDatingProfileDTO, DevDatingProfile>().ReverseMap();

            CreateMap<DevDatingSwipeAction, DevDatingSwipeActionDTO>().ReverseMap();
        }
    }
}
