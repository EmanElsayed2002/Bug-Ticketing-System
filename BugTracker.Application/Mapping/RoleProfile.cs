using AutoMapper;
using BugTracker.Data.models.Identity;
using School.Application.DTOs.User;

namespace BugTracker.Application.Mapping
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleResponse>().ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name));
            CreateMap<Role, RoleResponse>().ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
