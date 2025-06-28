namespace Application.Definition.GetAll;

public class GetDefinitionViewModel
{
    public Guid Id { get; set; }
    
    public string Word { get; set; }
    
    public string Meaning { get; set; }
    
    public Guid? BlobId { get; set; }
    
    public Guid DictionaryId { get; set; }
}