namespace Application.GeneralQuiz.Create;

public class CreateGeneralQuizResponseDto
{
    public Guid CardId { get; set; }
    public string? Name { get; set; }
    public virtual ICollection<CreateGeneralTestUnitViewModel> TestUnits { get; set; }
}