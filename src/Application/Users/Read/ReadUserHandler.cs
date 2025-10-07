using Infrastructure.Errors;
using Infrastructure.Security;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Read
{
    public class ReadUserHandler : IRequestHandler<ReadUserRequestDTO, ReadUserResponseDTO>
    {
        private readonly LearnContext _context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMediator _mediator;

        public ReadUserHandler(LearnContext context, IJwtTokenGenerator jwtTokenGenerator, IMediator mediator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mediator = mediator;
        }

        public async Task<ReadUserResponseDTO> Handle(ReadUserRequestDTO message, CancellationToken cancellationToken)
        {

            var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == message.Email && !x.IsDeleted, cancellationToken);
            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { User = Constants.NOT_FOUND });
            }

            var roles = string.Empty;
            foreach (var role in user.UserRoles)
            {
                roles += role.Role.RoleName + ", ";
            }
            var userToken = await _jwtTokenGenerator.CreateToken(user.Email, roles,120);
            var token = new JwtSecurityTokenHandler().ReadJwtToken(userToken);


            return new ReadUserResponseDTO()
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = userToken,
                TokenValidTo = token.ValidTo,
                Has2FAuthEnabled = user.IsGoogleAuthEnabled
            };
        }
    }
}
