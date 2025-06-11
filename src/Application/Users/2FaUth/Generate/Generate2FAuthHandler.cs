using Google.Authenticator;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using Infrastructure.Security;
using MediatR;
using System.Net;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Users._2FaUth.Generate
{
    public class Generate2FAuthHandler : IRequestHandler<Generate2FAuthRequestDTO, Generate2FAuthResponseDTO>
    {
        private readonly LearnContext _context;
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IPasswordHasher _passwordHasher;

        public Generate2FAuthHandler(LearnContext context, IMediator mediator, ICurrentUserAccessor currentUserAccessor, IPasswordHasher passwordHasher)
        {
            _context = context;
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
            _passwordHasher = passwordHasher;
        }

        public async Task<Generate2FAuthResponseDTO> Handle(Generate2FAuthRequestDTO message, CancellationToken cancellationToken)
        {
            var currentEmail = _currentUserAccessor.GetCurrentEmail();
            var person = await _context.Users.Where(x => x.Email == currentEmail && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

            PropertyChecker.CheckNullAndThrow404(person);

            if (!person.Hash.SequenceEqual(_passwordHasher.Hash(message.Password, person.Salt)))
            {
                throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
            }

            if (person.IsGoogleAuthEnabled && !message.Retry)
            {
                throw new RestException(HttpStatusCode.BadRequest,
                    new { Error = "User already has 2-Factor Authentication enabled!" });
            }

            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            person.GoogleAuthKey = key;

            await _context.SaveChangesAsync(cancellationToken);

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            SetupCode setupInfo = tfa.GenerateSetupCode("LearnIt", person.Email, key, false);

            return new Generate2FAuthResponseDTO()
            {
                ManualEntrySetupCode = setupInfo.ManualEntryKey,
                qrCodeImageURL = setupInfo.QrCodeSetupImageUrl
            };
        }
    }
}
