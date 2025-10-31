using Infrastructure.Quiz.AnswersResultBuilder;

namespace Application.Quiz.Answer.CheckAnswer;

public class CheckAnswerResponseDto
{
    public int CorrectAnswers { get; set; }
    public int IncorrectAnswers { get; set; }
    public string AchievementDescription {get;set;}
}