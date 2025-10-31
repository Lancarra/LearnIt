using Domain.Models.Quiz;
using MediatR;

namespace Application.Quiz;

public class CreateQuizRequestDto : IRequest<CreateQuizResponseDto>
{
    public Guid DictionaryId  { get; set; }
    public int QuestionsCount { get; set; }
    public string? Name { get; set; }
}