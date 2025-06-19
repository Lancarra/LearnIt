using System.Net;
 using Infrastructure.Database;
 using Infrastructure.Errors;
 using Infrastructure.Security;
 using MediatR;
 using Microsoft.EntityFrameworkCore;
 
 namespace Application.Users.GetById;
 
 public class GetUserHandler : IRequestHandler<GetUserRequestDto, GetUserResponseDto>
 {
     private readonly IMediator _mediator;
     private readonly LearnContext _context;
     private readonly IPasswordHasher _passwordHasher;
     
     public GetUserHandler( IMediator mediator, LearnContext context, IPasswordHasher passwordHasher)
     {
         _mediator = mediator;
         _context = context;
         _passwordHasher = passwordHasher;
     }
     
     public async Task<GetUserResponseDto> Handle(GetUserRequestDto request, CancellationToken cancellationToken)
     {
         var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && !u.IsDeleted, cancellationToken);
         if (user == null)
         {
             throw new RestException(HttpStatusCode.NotFound, new { User = "User not found" });
         }

         return new GetUserResponseDto
         {
             UserId = user.UserId,
             Email = user.Email,
             Username = user.Username,
             BlobId = user.BlobId,
         };
     }
 }