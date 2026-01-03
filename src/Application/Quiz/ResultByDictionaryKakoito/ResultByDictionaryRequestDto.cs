using MediatR;

namespace Application.Quiz.ResultByDictionaryKakoito;

public class ResultByDictionaryRequestDto : IRequest<ResultByDictionaryResponseDto>
{
    public int TeacherId { get; set; }
    public Guid DictionaryId { get; set; }
}