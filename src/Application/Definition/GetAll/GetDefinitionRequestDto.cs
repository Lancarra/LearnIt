using Application.Definition.Get;
using MediatR;

namespace Application.Definition.GetAll;

public class GetDefinitionRequestDto : IRequest<GetDefinitionResponseDto>
{
    public Guid DictionaryId { get; set; }
}