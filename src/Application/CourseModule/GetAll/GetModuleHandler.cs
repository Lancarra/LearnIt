using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CourseModule.GetAll;

public class GetModuleHandler : IRequestHandler<GetModuleRequestDto, GetModuleResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public GetModuleHandler(IMediator mediator, LearnContext context, ICurrentUserAccessor userAccessor)
    {
        _mediator = mediator;
        _context = context;
        _userAccessor = userAccessor;
    }


    public async Task<GetModuleResponseDto> Handle(GetModuleRequestDto request, CancellationToken cancellationToken)
    {

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(),
            cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);

        var role = _userAccessor.GetCurrentRoles();
        if (role.Contains("Student"))
        {
            var studentModules = await _context.CourseModules.Where(cm => cm.Students.Contains(user))
                .ToListAsync(cancellationToken);
            PropertyChecker.CheckNullAndThrow404(studentModules);

            var response = new List<GetModuleViewModel>();
            foreach (var module in studentModules)
            {
                var moduleResponse = new GetModuleViewModel()
                {
                    Id = module.Id,
                    Name = module.Name,
                    UserId = module.UserId,
                    Description = module.Description,
                    LearnLevel = module.LearnLevel,
                };
                response.Add(moduleResponse);
            }

            return new GetModuleResponseDto(response)
            {
                Count = response.Count
            };
        }

        var modules = await _context.CourseModules
                                                .Include(cm => cm.User)
                                                .Include(cm => cm.Folders)!
                                                .ThenInclude(f => f.Dictionaries)
            .Select(cm => new
            {
                cm.Id,
                cm.Name,
                cm.UserId,
                cm.Description,
                cm.LearnLevel,
                Author = cm.User.Username,
                DictionaryIds = cm.Folders
                    .SelectMany(f => f.Dictionaries.Select(d => d.Id))
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        PropertyChecker.CheckNullAndThrow404(modules);

        var allDictionaryIds = modules
            .SelectMany(m => m.DictionaryIds)
            .Where(id => id != null)
            .Cast<Guid>()
            .Distinct()
            .ToList();

        var quizCount = 0;
        if (allDictionaryIds.Any())
        {
            var dictionaryIdsList = allDictionaryIds; 
    
            quizCount = await _context.TestCards
                .Where(tc => dictionaryIdsList.Contains(tc.DictionaryId ?? Guid.Empty))
                .CountAsync(cancellationToken);
        }
        else
        {
            quizCount = 0;
        }

        var modulesResponse = modules.Select(module => new GetModuleViewModel()
        {
            Id = module.Id,
            Name = module.Name,
            UserId = module.UserId,
            Author = module.Author,
            Description = module.Description,
            LearnLevel = module.LearnLevel
        }).ToList();

        return new GetModuleResponseDto(modulesResponse)
        {
            Count = modulesResponse.Count,
            QuizCount = quizCount,
        };
    }
}