namespace Application.Definition.Update;

public class UpdateDefinitionResponseDto
{
    public Guid Id { get; set; }
    public string Word { get; set; }
    public string Meaning { get; set; }
    public Guid? BlobId { get; set; }
    public Guid DictionaryId { get; set; }
    public string? ImageURL { get; set; }
}