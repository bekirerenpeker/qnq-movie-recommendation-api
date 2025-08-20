using FluentValidation;
using MovieRecommendation.Dtos.Review;

namespace MovieRecommendation.Validators.Review;

public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.Comment).NotEmpty().WithMessage("comment cannot be empty");
        RuleFor(x => x.MovieId).NotEmpty().WithMessage("you need to specify a movie");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("you need to specify a user");
    }
}