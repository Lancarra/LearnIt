using MediatR;

namespace Application.Quiz.GetCards;

public class GetAllTestCardByDictionaryIdRequestDto : IRequest<GetAllTestCardByDictionaryIdResponseDto>
{
    public Guid DictionaryId { get; set; }
}