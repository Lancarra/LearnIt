using System.Net;
using Domain.Models.Quiz;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using Infrastructure.Quiz;
using MediatR;

namespace Application.Quiz;

public class CreateQuizHandler : IRequestHandler<CreateQuizRequestDto, CreateQuizResponseDto>
{
    private readonly ITestingBuilder _builder;
    private readonly ICurrentUserAccessor _userAccessor;
    
    public CreateQuizHandler(ITestingBuilder builder, ICurrentUserAccessor userAccessor)
    {
        _builder = builder;
        _userAccessor = userAccessor;
    }

    public async Task<CreateQuizResponseDto> Handle(CreateQuizRequestDto request, CancellationToken cancellationToken)
    {
        var roles = _userAccessor.GetCurrentRoles();
        PropertyChecker.CheckNullAndThrow404(roles);
        
        if (!roles.Contains("Admin") && !roles.Contains("Teacher"))
        {
            throw new RestException(HttpStatusCode.Unauthorized, new {Message = "You are haven't permission to perform this action"});
        }
        var result = await _builder.CreateQuiz(request.TestUnits, cancellationToken);

        var response = new CreateQuizResponseDto()
        {
            CardId = result.Id,
            TestUnits = new List<CreateTestUnitViewModel>()
        };
        foreach (var testUnitDto in result.TestUnits)
        {
            response.TestUnits.Add(new CreateTestUnitViewModel()
            {
                TestUnitId = testUnitDto.Id,
                AdditionalAnswers =  testUnitDto.AdditionalAnswers,
                DefinitionId = testUnitDto.DefinitionId,
                TestCardId =  testUnitDto.TestCardId,
            });
        }
        return response;    
    }
}