namespace Domain.Models;

public class Definition
{
    public Guid Id { get; set; }
    
    public string Word { get; set; }
    
    public string Meaning { get; set; }
    
    public string? BlobURL { get; set; }
    
    public virtual LearnWordDictionary Dictionary { get; set; }
    
    public Guid DictionaryId { get; set;}
}