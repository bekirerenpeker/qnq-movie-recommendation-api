using FluentValidation;
using MovieRecommendation.Dtos.Movie;

namespace MovieRecommendation.Validators.Movie;

public class CreateMovieDtoValidator : AbstractValidator<CreateMovieDto>
{
    public CreateMovieDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.DurationMins).NotEmpty().WithMessage("Duration in minutes required");
        RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("Movie should at least have one category");
    }
}

public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}

public class SelectMoviesDtoValidator : AbstractValidator<SelectMoviesDto>
{
    public SelectMoviesDtoValidator()
    {
        RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("At least one category is required");
    }
}
