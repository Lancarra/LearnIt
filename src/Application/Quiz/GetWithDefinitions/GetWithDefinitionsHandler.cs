using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quiz.GetWithDefinitions;

public class GetWithDefinitionsHandler : IRequestHandler<GetWithDefinitionsRequestDto, GetWithDefinitionsResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;
    
    public GetWithDefinitionsHandler(LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }
    
    public async Task<GetWithDefinitionsResponseDto> Handle(GetWithDefinitionsRequestDto request, CancellationToken cancellationToken)
    {
        var roles = _userAccessor.GetCurrentRoles();
        PropertyChecker.CheckNullAndThrow404(roles);
        
        if (!roles.Contains("Admin") && !roles.Contains("Teacher"))
        {
            throw new RestException(HttpStatusCode.Unauthorized, new {Message = "You are haven't permission to perform this action"});
        }
        var card = await _context.TestCards.Include(tc => tc.TestUnits).ThenInclude(tu => tu.Definition).FirstOrDefaultAsync(c => c.Id == request.TestCardId, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(card);

        var response = new GetWithDefinitionsResponseDto
        {
            CardId = request.TestCardId,
            Name = card.Name,
            TestUnits = new List<GetWithDefinitionsViewModel>(),
            DefinitionsCount = card.TestUnits.Count
        };

        foreach (var u in card.TestUnits)
        {
            var definition = new DefinitionViewModel
            {
                Id = u.DefinitionId,
                Word = u.Definition.Word,
                Meaning = u.Definition.Meaning,
                BlobId = u.Definition.BlobId,
                ImageURL = u.Definition.ImageUrl,
                DictionaryId = u.Definition.DictionaryId,
            };
            response.TestUnits.Add(new GetWithDefinitionsViewModel
            {
                TestUnitId = u.Id,
                AdditionalAnswers = u.AdditionalAnswers,
                Definition = definition,
                TestCardId = u.TestCardId
            });
        }
        return response;
    }
}