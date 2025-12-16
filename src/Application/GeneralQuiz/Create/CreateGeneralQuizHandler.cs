using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Quiz.GeneralTest;
using MediatR;

namespace Application.GeneralQuiz.Create;

public class CreateGeneralQuizHandler : IRequestHandler<CreateGeneralQuizRequestDto, CreateGeneralQuizResponseDto>
{
    private readonly IGeneralTest _builder;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly LearnContext _context;
    
    public CreateGeneralQuizHandler(IGeneralTest builder, ICurrentUserAccessor userAccessor, LearnContext context)
    {
        _builder = builder;
        _userAccessor = userAccessor;
        _context = context;
    }
    public async Task<CreateGeneralQuizResponseDto> Handle(CreateGeneralQuizRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _builder.CreateGeneralTest(request.Name, request.ModuleId, request.DefinitionCount, cancellationToken);
        var response = new CreateGeneralQuizResponseDto()
        {
            CardId = result.Id,
            Name = result.Name,
            TestUnits = new List<CreateGeneralTestUnitViewModel>()
        };
        foreach (var testUnitDto in result.TestUnits)
        {
            response.TestUnits.Add(new CreateGeneralTestUnitViewModel()
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