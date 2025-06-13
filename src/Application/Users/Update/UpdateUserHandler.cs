using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Update;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequestDto, UpdateUserResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public UpdateUserHandler( IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    
    public async Task<UpdateUserResponseDto> Handle(UpdateUserRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && !x.IsDeleted, cancellationToken);

        if (user == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
        }
        
        user.Email = request.Email;
        user.Username = request.Username;
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new UpdateUserResponseDto { UserId = user.UserId };
    }
}