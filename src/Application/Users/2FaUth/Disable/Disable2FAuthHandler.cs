using Google.Authenticator;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Users._2FaUth.Disable
{
    public class Disable2FAuthHandler : IRequestHandler<Disable2FAuthRequestDTO, Unit>
    {
        private readonly LearnContext _context;
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IPasswordHasher _passwordHasher;

        public Disable2FAuthHandler(LearnContext context, IMediator mediator, ICurrentUserAccessor currentUserAccessor, IPasswordHasher passwordHasher)
        {
            _context = context;
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(Disable2FAuthRequestDTO message, CancellationToken cancellationToken)
        {
            var currentEmail = _currentUserAccessor.GetCurrentEmail();
            var person = await _context.Users.Where(x => x.Email == currentEmail && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (!person.Hash.SequenceEqual(_passwordHasher.Hash(message.Password, person.Salt)))
            {
                throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid password or the authenticator code!" });
            }

            if (person.IsGoogleAuthEnabled)
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                bool result = tfa.ValidateTwoFactorPIN(person.GoogleAuthKey, message.GoogleCode);
                if (!result)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid password or the authenticator code!" });
                }
            }

            person.GoogleAuthKey = "";
            person.IsGoogleAuthEnabled = false;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
