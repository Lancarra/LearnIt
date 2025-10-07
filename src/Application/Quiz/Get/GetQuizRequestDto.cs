using MediatR;

namespace Application.Quiz.Get;

public class GetQuizRequestDto : IRequest<GetQuizResponseDto>
{
    public Guid TestCardId { get; set; }
}