using Domain.Models.Quiz;

namespace Domain.Models;

public class TestUnit
{
    public TestUnit()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public List<string> AdditionalAnswers {get; set;}
    public virtual Definition Definition { get; set; }
    public Guid DefinitionId { get; set; }
    public Guid DictionaryId { get; set; }

    public virtual TestCard TestCard { get; set; }
    public Guid TestCardId { get; set; }
    public virtual ICollection<TestUnitAnswers> TestUnitAnswers { get; set; }
}