namespace Domain.Models.Quiz;

public class UpdateTestUnitViewModel
{
    public Guid Id { get; set; }
    public List<string> AdditionalAnswers {get; set;}
}