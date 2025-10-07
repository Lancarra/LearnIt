namespace Application.Quiz.Update;

public class UpdateQuizResponseDto
{
    public Guid CardId { get; set; }
    public virtual ICollection<CreateTestUnitViewModel> TestUnits { get; set; }
}