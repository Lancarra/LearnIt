namespace Domain.Models.Quiz;

public class UpdateTestUnitViewModel
{
    public Guid Id { get; set; }
    public string? Word { get; set; }
    public string? Meaning { get; set; }
    public List<string> AdditionalAnswers {get; set;}
}