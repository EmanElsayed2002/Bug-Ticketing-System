using AutoMapper;
using BugTracker.Application.DTOs.Bug.DTO;
using BugTracker.Data.models;

namespace BugTracker.Application.Mapping
{
    public class BugProfile : Profile
    {
        public BugProfile()
        {
            CreateMap<AddBugDTO, Bug>().ReverseMap();
            CreateMap<Bug, BugDtoResponse>().ReverseMap();
        }
    }
}
