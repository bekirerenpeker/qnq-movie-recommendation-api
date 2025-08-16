namespace MovieRecommendation.Dtos.Movie;

public class CategoryDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = "";
    
    public List<MovieDto> Movies { get; set; } = [];
}