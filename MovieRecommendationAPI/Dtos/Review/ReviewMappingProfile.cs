using AutoMapper;
using MovieRecommendation.Data.Review;

namespace MovieRecommendation.Dtos.Review;

public class ReviewMappingProfile : Profile
{
    public ReviewMappingProfile()
    {
        CreateMap<ReviewData, ReviewDto>();
    }
}