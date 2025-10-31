namespace Application.Quiz.Answer.CheckSingleAnswer;

public class CheckSingleAnswerResponseDto
{
    public string UserAnswer { get; set; }
    public bool IsCorrect { get; set; }
    
    public string DefinitionWord { get; set; }
    
    public string DefinitionMeaning { get; set; }
    
}