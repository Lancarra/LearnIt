namespace Application.LearnWordDictionary.Update;

public class UpdateDictionaryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ParentFolderId { get; set; }
}