namespace Domain.Models.Quiz;

public class TestUnitAnswers
{
    public TestUnitAnswers()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Answer { get; set; }
    public virtual TestUnit TestUnit { get; set; }
    public Guid TestUnitId { get; set; }
    public virtual TestCardAnswer TestCardAnswer { get; set; }
    public Guid TestCardAnswerId { get; set; }
}