using Infrastructure.CurrentUserAccessor;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using MediatR;
using System.Net;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Users._2FaUth.Verify
{
    public class Verify2FAuthHandler : IRequestHandler<Verify2FAuthRequestDTO, Unit>
    {
        private readonly LearnContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public Verify2FAuthHandler(LearnContext context, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<Unit> Handle(Verify2FAuthRequestDTO message,CancellationToken cancellationToken)
        {
            var currentEmail = _currentUserAccessor.GetCurrentEmail();
            var user = await _context.Users.Where(x => x.Email == currentEmail && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
            PropertyChecker.CheckNullAndThrow404(user);

            if (user.GoogleAuthKey.Length <= 0)
            {
                throw new RestException(HttpStatusCode.Unauthorized, new { Error = "User does not have 2FA key generated!" });

            }
            //TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            //bool result = tfa.ValidateTwoFactorPIN(user.GoogleAuthKey, message.GoogleCode);
            //if (!result)
            //{
            //    throw new RestException(HttpStatusCode.BadRequest, new { Error = "Invalid authenticator code!" });
            //}
            user.IsGoogleAuthEnabled = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
