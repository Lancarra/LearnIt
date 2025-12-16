namespace Application.Quiz.GetWithDefinitions;

public class GetWithDefinitionsViewModel
{
    public Guid TestUnitId { get; set; }
    public List<string> AdditionalAnswers { get; set; } = new();
    public DefinitionViewModel Definition { get; set; }
    public Guid TestCardId { get; set; }
}