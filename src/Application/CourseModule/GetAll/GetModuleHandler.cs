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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail(), cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var modules = await _context.CourseModules.Where(cm => cm.UserId == user.UserId).ToListAsync(cancellationToken);
        PropertyChecker.CheckNullAndThrow404(modules);
        
        var modulesResponse = new List<GetModuleViewModel>();
        foreach (var module in modules)
        {
            var moduleResponse = new GetModuleViewModel()
            {
                Id = module.Id, 
                Name = module.Name,
                UserId = module.UserId
            };
            modulesResponse.Add(moduleResponse);
        }

        return new GetModuleResponseDto(modulesResponse);
    }
}