namespace Application.GeneralQuiz.Create;

public class CreateGeneralTestUnitViewModel
{
    public Guid TestUnitId { get; set; }
    public List<string> AdditionalAnswers {get; set;}
    public Guid DefinitionId { get; set; }
    public Guid TestCardId { get; set; }
}