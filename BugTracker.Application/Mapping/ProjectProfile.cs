using AutoMapper;
using BugTracker.Application.DTOs.Projects.DTO;
using BugTracker.Data.models;

namespace BugTracker.Application.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectDtoRequest, Project>().ForMember(dest => dest.Bugs, opt => opt.MapFrom(src => src.Bugs)).ReverseMap();
            CreateMap<Project, ProjectDtoResponse>().ForMember(dest => dest.Bugs, opt => opt.MapFrom(src => src.Bugs)).ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProjectId)).ReverseMap();


        }
    }
}
