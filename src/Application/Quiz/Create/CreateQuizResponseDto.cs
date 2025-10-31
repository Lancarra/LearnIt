namespace Application.Quiz;

public class CreateQuizResponseDto
{
    public Guid CardId { get; set; }
    public string? Name { get; set; }
    public virtual ICollection<CreateTestUnitViewModel> TestUnits { get; set; }
}