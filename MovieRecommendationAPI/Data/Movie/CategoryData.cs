namespace MovieRecommendation.Data.Movie;

public class CategoryData
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = "";
    
    public List<MovieData> Movies { get; set; } = [];
}