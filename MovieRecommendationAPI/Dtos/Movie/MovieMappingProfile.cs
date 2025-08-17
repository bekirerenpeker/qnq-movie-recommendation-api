using AutoMapper;
using MovieRecommendation.Data.Movie;

namespace MovieRecommendation.Dtos.Movie;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<MovieData, MovieDto>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id)));
        
        CreateMap<CategoryData, CategoryDto>()
            .ForMember(dest => dest.MovieIds, opt => opt.MapFrom(src => src.Movies.Select(c => c.Id)));
    }
}