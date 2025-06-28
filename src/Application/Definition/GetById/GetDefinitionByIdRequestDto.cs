using MediatR;

namespace Application.Definition.GetById;

public class GetDefinitionByIdRequestDto : IRequest<GetDefinitionByIdResponseDto>
{
    public Guid DefinitionId { get; set; }
}