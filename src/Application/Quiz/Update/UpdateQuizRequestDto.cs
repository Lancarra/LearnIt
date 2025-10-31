using Domain.Models.Quiz;
using MediatR;

namespace Application.Quiz.Update;

public class UpdateQuizRequestDto : IRequest<UpdateQuizResponseDto>
{
    public Guid CardId { get; set; } 
    public string? Name { get; set; }
    public List<UpdateTestUnitViewModel> TestUnits {get; set;}
}