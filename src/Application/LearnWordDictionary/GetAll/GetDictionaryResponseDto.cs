namespace Application.LearnWordDictionary.GetAll;

public class GetDictionaryResponseDto
{
    public IEnumerable<GetDictionaryViewModel> Dictionary { get; set; }

    public GetDictionaryResponseDto(List<GetDictionaryViewModel> dictionary)
    {
        Dictionary = dictionary;
    }
}