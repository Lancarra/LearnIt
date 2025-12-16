using MediatR;

namespace Application.Quiz.GetWithDefinitions;

public class GetWithDefinitionsRequestDto  : IRequest<GetWithDefinitionsResponseDto>
{
    public Guid TestCardId { get; set; }
}