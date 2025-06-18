using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Delete;

public class DeleteUserHandler : IRequestHandler<DeleteUserRequestDto, DeleteUserResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public DeleteUserHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    
    public async Task<DeleteUserResponseDto> Handle(DeleteUserRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && !u.IsDeleted, cancellationToken);
        if (user == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new { User = "User not found" });
        }
        
        user.IsDeleted = true;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new DeleteUserResponseDto();
    }
}