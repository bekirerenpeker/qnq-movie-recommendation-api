using AutoMapper;
using MovieRecommendation.Data.Auth;

namespace MovieRecommendation.Dtos.Auth;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<UserData, UserDto>()
            .ForMember(dest => dest.WatchedMovieIds, opt => opt.MapFrom(src => src.WatchedMovies.Select(m => m.Id)));
        
        CreateMap<CreateUserDto, UserData>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        
        CreateMap<UpdateUserDto, UserData>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<RegisterDto, CreateUserDto>();
    }
}