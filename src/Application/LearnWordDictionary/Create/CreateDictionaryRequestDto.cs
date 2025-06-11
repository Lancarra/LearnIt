using MediatR;

namespace Application.LearnWordDictionary.Create;

public class CreateDictionaryRequestDto : IRequest<CreateDictionaryResponseDto>
{
    public string Name { get; set; }
    public Guid ParentFolderId { get; set; }
}