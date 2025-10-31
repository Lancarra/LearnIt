namespace Infrastructure.Quiz.AnswersResultBuilder;

public class ResultBuilderModel
{
    public Guid TestCardAnswerId { get; set; }
    public int CorrectAnswers { get; set; }
    public int IncorrectAnswers { get; set; }
    public Guid CardId {get;set;}
    public string AchievementDescription {get;set;}
}