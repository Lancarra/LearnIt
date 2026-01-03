using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using Infrastructure.Quiz;
using MediatR;

namespace Application.Quiz.Update;

public class UpdateQuizHandler : IRequestHandler<UpdateQuizRequestDto, UpdateQuizResponseDto>
{
    private readonly ITestingBuilder _builder;
    private readonly ICurrentUserAccessor _userAccessor;

    public UpdateQuizHandler(ITestingBuilder builder, ICurrentUserAccessor userAccessor)
    {
        _builder = builder;
        _userAccessor = userAccessor;
    }

    public async Task<UpdateQuizResponseDto> Handle(UpdateQuizRequestDto request, CancellationToken cancellationToken)
    {
        var roles = _userAccessor.GetCurrentRoles();
        PropertyChecker.CheckNullAndThrow404(roles);

        if (!roles.Contains("Admin") && !roles.Contains("Teacher"))
        {
            throw new RestException(HttpStatusCode.Unauthorized,
                new { Message = "You are haven't permission to perform this action" });
        }

        var result = await _builder.UpdateQuiz(request.CardId, request.Name, request.TestUnits, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(result);
        
        var response = new UpdateQuizResponseDto
        {
            CardId = result.Id,
            Name = result.Name,
            TestUnits = new List<CreateTestUnitViewModel>()
        };

        foreach (var tu in result.TestUnits)
        {
            response.TestUnits.Add(new CreateTestUnitViewModel
            {
                TestUnitId = tu.Id,
                
                AdditionalAnswers = tu.AdditionalAnswers,
                DefinitionId = tu.DefinitionId,
                /*
                TestCardId = tu.TestCardId,
            */
            });
        }
        return response;
    }
}