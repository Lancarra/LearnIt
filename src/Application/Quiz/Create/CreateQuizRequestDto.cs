using Domain.Models.Quiz;
using MediatR;

namespace Application.Quiz;

public class CreateQuizRequestDto : IRequest<CreateQuizResponseDto>
{
    public List<TestUnitRequestDto> TestUnits { get; set; } = new(); 
}