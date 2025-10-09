namespace Application.Quiz.Get;

public class GetQuizViewModel
{
    public Guid TestUnitId { get; set; }
    public List<string> AdditionalAnswers { get; set; } = new();
    public Guid DefinitionId { get; set; }
    public Guid TestCardId { get; set; }
}