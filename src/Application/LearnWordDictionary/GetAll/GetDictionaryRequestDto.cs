using MediatR;

namespace Application.LearnWordDictionary.GetAll;

public class GetDictionaryRequestDto : IRequest<GetDictionaryResponseDto>
{
    public Guid ParentFolderId { get; set; }
}