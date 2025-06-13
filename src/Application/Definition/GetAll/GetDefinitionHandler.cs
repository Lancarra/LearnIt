using Application.Definition.Get;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Definition.GetAll;

public class GetDefinitionHandler : IRequestHandler<GetDefinitionRequestDto, GetDefinitionResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetDefinitionHandler(IMediator mediator, LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _mediator = mediator;
        _context = context;
        _userAccessor = userAccessor;
    }
    public async Task<GetDefinitionResponseDto> Handle(GetDefinitionRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var definitions = await _context.Definitions.Where(d => d.DictionaryId == request.DictionaryId).ToListAsync(cancellationToken);
        PropertyChecker.CheckNullAndThrow404(definitions);
        
        var definitionsResponse = new List<GetDefinitionViewModel>();
        foreach (var definition in definitions)
        {
            var definitionResponse = new GetDefinitionViewModel()
            {
                Id = definition.Id,
                Word = definition.Word,
                Meaning = definition.Meaning,
                BlobURL = definition.BlobURL,
                DictionaryId = definition.DictionaryId
            };
                definitionsResponse.Add(definitionResponse);
        }
        return new GetDefinitionResponseDto(definitionsResponse);
    }
}