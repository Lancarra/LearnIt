using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Definition.GetById;

public class GetDefinitionByIdHandler : IRequestHandler<GetDefinitionByIdRequestDto, GetDefinitionByIdResponseDto>
{   
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetDefinitionByIdHandler(IMediator mediator, LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _mediator = mediator;
        _context = context;
        _userAccessor = userAccessor;
    }
    public async Task<GetDefinitionByIdResponseDto> Handle(GetDefinitionByIdRequestDto byIdRequest, CancellationToken cancellationToken)
    {
        var definitions = await _context.Definitions.FirstOrDefaultAsync(d => d.Id == byIdRequest.DefinitionId, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(definitions);
        return new GetDefinitionByIdResponseDto()
        {
            Id = definitions.Id,
            Word = definitions.Word,
            Meaning = definitions.Meaning,  
            BlobId = definitions.BlobId,
            ImageURL = definitions.ImageUrl,
            DictionaryId = definitions.DictionaryId
        };
    }
}