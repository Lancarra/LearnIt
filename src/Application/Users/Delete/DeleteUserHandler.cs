using System.Net;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Delete;

public class DeleteUserHandler : IRequestHandler<DeleteUserRequestDto, bool>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    
    public DeleteUserHandler(IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    
    public async Task<bool> Handle(DeleteUserRequestDto request, CancellationToken cancellationToken)
    {
        var userToDelete = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted, cancellationToken);
        if (userToDelete == null) throw new RestException(HttpStatusCode.NotFound, new { message = "No User with this email found!" });

        //Elements which should be deleted when deleting User


        //SoftDelete
        userToDelete.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}