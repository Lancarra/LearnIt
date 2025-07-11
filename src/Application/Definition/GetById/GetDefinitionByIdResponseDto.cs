namespace Application.Definition.GetById;

public class GetDefinitionByIdResponseDto
{
    public Guid Id { get; set; }
    
    public string Word { get; set; }
    
    public string Meaning { get; set; }
    
    public Guid? BlobId { get; set; }
    
    public string? ImageURL { get; set; }
    
    public Guid DictionaryId { get; set; }
}