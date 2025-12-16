using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CourseModule.GetByTeacherId;

public class GetByTeacherIdHandler : IRequestHandler<GetByTeacherIdRequestDto, GetByTeacherIdResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetByTeacherIdHandler(IMediator mediator, LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _mediator = mediator;
        _context = context;
        _userAccessor = userAccessor;
    }


    public async Task<GetByTeacherIdResponseDto> Handle(GetByTeacherIdRequestDto request, CancellationToken cancellationToken)
    {
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.TeacherId, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
       
        var modules = await _context.CourseModules.Include(cm => cm.Folders)
                                                                    .ThenInclude(f => f.Dictionaries)
                                                                    .Include(u => u.Students)
                                                                    .Where(cm => cm.UserId == request.TeacherId).ToListAsync(cancellationToken);
        PropertyChecker.CheckNullAndThrow404(modules);
        
        var modulesResponse = new List<GetByTeacherIdViewModel>();
        var studentCount = 0;
        foreach (var module in modules)
        {
            var dictionaryCount = 0;
            foreach (var folder in module.Folders)
            {
                dictionaryCount += folder.Dictionaries.Count;
            }
            studentCount += module.Students.Count;
            
            var moduleResponse = new GetByTeacherIdViewModel()
            {
                Id = module.Id, 
                Name = module.Name,
                Description = module.Description,
                UserId = module.UserId,
                DictionaryCount = dictionaryCount,
                LearnLevel = module.LearnLevel,
            };
            modulesResponse.Add(moduleResponse);
        }

        return new GetByTeacherIdResponseDto(modulesResponse)
        {
            Count = modulesResponse.Count,
            StudentCount = studentCount
        };
    }
}