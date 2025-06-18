using MediatR;

namespace Application.Definition.Create;

public class CreateDefinitionRequestDto : IRequest<CreateDefinitionResponseDto>
{
    public string Word { get; set; }
    public string Meaning { get; set; }
    public string? BlobURL { get; set; }
    public Guid DictionaryId { get; set; }
}