namespace Application.Quiz;

public class CreateTestUnitViewModel
{
    public Guid TestUnitId { get; set; }
    public List<string> AdditionalAnswers {get; set;}
    public Guid DefinitionId { get; set; }
    public Guid TestCardId { get; set; }
}