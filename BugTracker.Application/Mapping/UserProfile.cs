using AutoMapper;
using BugTracker.Data.models.Identity;
using School.Application.DTOs.User;
using SurveyBasket.API.DTOs.User;

namespace BugTracker.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserProfileResponse, User>();
            CreateMap<UpdateProfileRequest, User>();
        }
    }
}
