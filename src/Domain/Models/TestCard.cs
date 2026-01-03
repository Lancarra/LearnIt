using Domain.Models.Quiz;

namespace Domain.Models;

public class TestCard
{
    public TestCard()
    {
        Id = Guid.NewGuid();
        Created =  DateTime.Now;
    }
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public virtual LearnWordDictionary Dictionary { get; set; }
    public Guid? DictionaryId { get; set; }
    public virtual User User { get; set; }
    public int UserId {get; set;}
    
    public virtual ICollection<User> AssignedUsers { get; set; }
    public virtual ICollection<TestUnit> TestUnits { get; set; }
    public virtual ICollection<TestCardAnswer> TestCardAnswers { get; set; }
}