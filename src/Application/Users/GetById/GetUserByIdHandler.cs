using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
 using Infrastructure.Errors;
 using Infrastructure.Security;
 using MediatR;
 using Microsoft.EntityFrameworkCore;
 
 namespace Application.Users.GetById;
 
 public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequestDto, GetUserByIdResponseDto>
 {
     private readonly IMediator _mediator;
     private readonly LearnContext _context;
     private readonly IPasswordHasher _passwordHasher;
     private readonly ICurrentUserAccessor _userAccessor;
     
     public GetUserByIdHandler( IMediator mediator, LearnContext context, IPasswordHasher passwordHasher, ICurrentUserAccessor userAccessor)
     {
         _mediator = mediator;
         _context = context;
         _passwordHasher = passwordHasher;
         _userAccessor = userAccessor;
     }
     
     public async Task<GetUserByIdResponseDto> Handle(GetUserByIdRequestDto request, CancellationToken cancellationToken)
     {
         var user = await _context.Users.Include(u => u.Achievement)
                                        .Include(u => u.UserRoles)
                                        .ThenInclude(ur => ur.Role)
                                        .FirstOrDefaultAsync(u => u.UserId == request.UserId && !u.IsDeleted, cancellationToken);
         if (user == null)
         {
             throw new RestException(HttpStatusCode.NotFound, new { User = "User not found" });
         }

         var roles = string.Join(",", user.UserRoles.Select(ur => ur.Role.RoleName));
         
         return new GetUserByIdResponseDto
         {
             UserId = user.UserId,
             Email = user.Email,
             Username = user.Username,
             BlobId = user.BlobId,
             RoleName = string.IsNullOrWhiteSpace(roles) ? "Empty" : roles,
             Achievement = user.Achievement != null 
                 ? user.Achievement.Name 
                 : "You haven't completed a single dictionary",
             TeachersId = user.TeachersId,
             StudentsId = user.StudentsId
         };
     }
 }