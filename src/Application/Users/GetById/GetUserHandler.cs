using System.Net;
using Infrastructure.CurrentUserAccessor;
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
     private readonly ICurrentUserAccessor _userAccessor;
     
     public GetUserHandler( IMediator mediator, LearnContext context, IPasswordHasher passwordHasher, ICurrentUserAccessor userAccessor)
     {
         _mediator = mediator;
         _context = context;
         _passwordHasher = passwordHasher;
         _userAccessor = userAccessor;
     }
     
     public async Task<GetUserResponseDto> Handle(GetUserRequestDto request, CancellationToken cancellationToken)
     {
         
         var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted, cancellationToken);
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