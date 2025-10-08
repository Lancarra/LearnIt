namespace Application.Quiz.Get;

public class GetQuizResponseDto
{
    public Guid CardId { get; set; }         
    public ICollection<GetQuizViewModel> TestUnits { get; set; } = new List<GetQuizViewModel>();
}