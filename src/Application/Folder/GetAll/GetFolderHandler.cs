using Application.Folder.Get;
using Application.Folder.GetAll;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CourseModule.Get;

public class GetFolderHandler : IRequestHandler<GetFolderRequestDto, GetFolderResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;
    
    public GetFolderHandler(IMediator mediator, LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _mediator = mediator;
        _context = context;
        _userAccessor = userAccessor;
    }
    
    public async Task<GetFolderResponseDto> Handle(GetFolderRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var folders = await _context.Folders.Where(f => f.CourseModuleId == request.CourseModuleId).ToListAsync(cancellationToken);
        PropertyChecker.CheckNullAndThrow404(folders);

        var foldersResponse = new List<GetFolderViewModel>();
        foreach (var folder in folders)
        {
            var folderResponse = new GetFolderViewModel()
            {
                Id = folder.Id,
                Name = folder.Name,
                CourseModuleId = folder.CourseModuleId
            };
            foldersResponse.Add(folderResponse);
        }
        return new GetFolderResponseDto(foldersResponse);
    }
}