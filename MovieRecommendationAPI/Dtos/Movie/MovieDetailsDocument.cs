using QuestPDF.Infrastructure;

namespace MovieRecommendation.Dtos.Movie;

public class MovieDetailsDocument: IDocument
{
    private readonly MovieDetailsDto _movieDetailsDtodto;

    public MovieDetailsDocument(MovieDetailsDto movieDetailsDto)
    {
        _movieDetailsDtodto = movieDetailsDto;
    }
    
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        throw new NotImplementedException();
    }
}