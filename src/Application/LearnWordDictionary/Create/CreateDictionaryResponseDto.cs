namespace Application.LearnWordDictionary.Create;

public class CreateDictionaryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ParentFolderId { get; set; }

}