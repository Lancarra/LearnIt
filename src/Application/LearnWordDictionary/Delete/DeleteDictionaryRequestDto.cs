using MediatR;

namespace Application.LearnWordDictionary.Delete;

public class DeleteDictionaryRequestDto : IRequest<DeleteDictionaryResponseDto>
{
    public Guid Id { get; set; }
}