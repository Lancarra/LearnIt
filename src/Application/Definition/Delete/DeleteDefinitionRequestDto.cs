using MediatR;

namespace Application.Definition.Delete;

public class DeleteDefinitionRequestDto : IRequest<DeleteDefinitionResponseDto>
{
    public Guid Id { get; set; }
}