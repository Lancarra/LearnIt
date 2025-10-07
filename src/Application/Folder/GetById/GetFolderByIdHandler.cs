using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Folder.GetById;

public class GetFolderByIdHandler : IRequestHandler<GetFolderByIdRequestDto, GetFolderByIdResponseDto>
{
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetFolderByIdHandler(LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<GetFolderByIdResponseDto> Handle(GetFolderByIdRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        var roles = _userAccessor.GetCurrentRoles();

        PropertyChecker.CheckNullAndThrow404(user);
        PropertyChecker.CheckNullAndThrow404(roles);

        if (!roles.Contains("Admin") && !roles.Contains("Teacher") && !roles.Contains("Student"))
        {
            throw new RestException(HttpStatusCode.Unauthorized, new { Message = "You are haven't permission to perform this action" });
        }

        var folder = await _context.Folders
            .Include(f => f.Dictionaries)  
            .Include(f => f.CourseModule)  
            .FirstOrDefaultAsync(f => f.Id == request.FolderId && f.CourseModule.UserId == user!.UserId, cancellationToken);

        PropertyChecker.CheckNullAndThrow404(folder);

        var dictionariesVm = new List<DictionaryViewModel>();
        foreach (var d in folder!.Dictionaries)
        {
            dictionariesVm.Add(new DictionaryViewModel
            {
                Id = d.Id,
                Name = d.Name 
            });
        }

        var folderVm = new GetFolderByIdViewModel
        {
            Id = folder.Id,
            Name = folder.Name,
            CourseModuleId = folder.CourseModuleId,
            Dictionaries = dictionariesVm
        };

        return new GetFolderByIdResponseDto(folderVm);
    }
}
