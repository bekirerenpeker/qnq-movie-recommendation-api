namespace MovieRecommendation.Dtos.Movie;

public class CategoryDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = "";

    public List<Guid> MovieIds { get; set; } = [];
}

public class CreateCategoryDto
{
    public string Name { get; set; } = "";
}

public class UpdateCategoryDto
{
    public string? Name { get; set; } = null;
}