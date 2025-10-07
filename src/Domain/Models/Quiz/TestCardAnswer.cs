namespace Domain.Models.Quiz;

public class TestCardAnswer
{
    public TestCardAnswer()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public virtual TestCard TestCard { get; set; }
    public Guid TestCardId { get; set; }
    public virtual ICollection<TestUnitAnswers> TestUnitAnswers { get; set; }
    public virtual User User { get; set; }
    public int UserId { get; set; }
}