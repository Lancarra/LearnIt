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
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);

        var folders = await _context.Folders
            .Include(f => f.CourseModule)
            .Where(f => f.CourseModuleId == request.CourseModuleId && f.CourseModule.UserId == user.UserId)
            .ToListAsync(cancellationToken);
    
        PropertyChecker.CheckNullAndThrow404(folders);

        var foldersResponse = folders.Select(folder => new GetFolderViewModel
        {
            Id = folder.Id,
            Name = folder.Name,
            CourseModuleId = folder.CourseModuleId
        }).ToList();

        return new GetFolderResponseDto(foldersResponse);
    }

}