using AutoMapper;
using MovieRecommendation.Data.Movie;

namespace MovieRecommendation.Dtos.Movie;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<MovieData, MovieDto>();
        CreateMap<CategoryData, CategoryDto>();
    }
}