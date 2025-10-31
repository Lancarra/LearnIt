using System.Net;
using Domain.Models.Quiz;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using Infrastructure.Quiz;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quiz;

public class CreateQuizHandler : IRequestHandler<CreateQuizRequestDto, CreateQuizResponseDto>
{
    private readonly ITestingBuilder _builder;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly LearnContext _context;
    
    public CreateQuizHandler(ITestingBuilder builder, ICurrentUserAccessor userAccessor, LearnContext context)
    {
        _builder = builder;
        _userAccessor = userAccessor;
        _context = context;
    }

    public async Task<CreateQuizResponseDto> Handle(CreateQuizRequestDto request, CancellationToken cancellationToken)
    {
        var roles = _userAccessor.GetCurrentRoles();
        PropertyChecker.CheckNullAndThrow404(roles);
        
        if (!roles.Contains("Admin") && !roles.Contains("Teacher") && !roles.Contains("Student"))
        {
            throw new RestException(HttpStatusCode.Unauthorized, new {Message = "You are haven't permission to perform this action"});
        }
        
        var definitions = await _context.Definitions
                    .Where(d => d.DictionaryId == request.DictionaryId)
                    .ToListAsync(cancellationToken);

        if (definitions.Count == 0)
        {
            throw new RestException(HttpStatusCode.BadRequest,
                new { Message = $"Dictionary {request.DictionaryId} has no definitions" });
        }
        
        var random = new Random();
        var usedIds = new HashSet<Guid>();
        var createQuizRequest = new List<TestUnitRequestDto>();

        while (createQuizRequest.Count < request.QuestionsCount && usedIds.Count < definitions.Count)
        {
            var definition = definitions[random.Next(definitions.Count)];
            if (usedIds.Add(definition.Id))
            {
                createQuizRequest.Add(new TestUnitRequestDto
                {
                    DefinitionId = definition.Id,
                    DictionaryId = definition.DictionaryId,
                });
            }
        }
        
        var result = await _builder.CreateQuiz(request.Name, createQuizRequest, cancellationToken);

        var response = new CreateQuizResponseDto()
        {
            CardId = result.Id,
            Name = result.Name,
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