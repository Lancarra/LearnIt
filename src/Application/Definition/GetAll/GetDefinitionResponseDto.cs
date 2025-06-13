using Application.Definition.GetAll;

namespace Application.Definition.Get;

public class GetDefinitionResponseDto
{
    public IEnumerable<GetDefinitionViewModel> Definitions { get; set; }
    public GetDefinitionResponseDto(List<GetDefinitionViewModel> definitions)
    {
        Definitions = definitions;
    }
}