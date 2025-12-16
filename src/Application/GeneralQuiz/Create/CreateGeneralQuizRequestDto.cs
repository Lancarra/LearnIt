using MediatR;

namespace Application.GeneralQuiz.Create;

public class CreateGeneralQuizRequestDto : IRequest <CreateGeneralQuizResponseDto>
{
    public string Name { get; set; }
    public Guid ModuleId { get; set; }
    public int DefinitionCount  { get; set; }
}