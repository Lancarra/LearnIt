using MediatR;

namespace Application.LearnWordDictionary.Update;

public class UpdateDictionaryRequestDto : IRequest<UpdateDictionaryResponseDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ParentFolderId { get; set; }
    public bool FinishStudy { get; set; }
}