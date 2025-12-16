namespace Application.LearnWordDictionary.GetAll;

public class GetDictionaryResponseDto
{
    public IEnumerable<GetDictionaryViewModel> Dictionary { get; set; }
    public int DictionaryCount { get; set; }
    public GetDictionaryResponseDto(List<GetDictionaryViewModel> dictionary, int dictionaryCount)
    {
        Dictionary = dictionary;
        DictionaryCount = dictionaryCount;
    }
}