using Domain;
using Google.Authenticator;
using Infrastructure.Errors;
using System.Net;

namespace Infrastructure.Helpers
{
    public static class TFAHelper
    {
        public static bool TwoFactorAuthentication(User user, string googleAuthCode)
        {

            if (user.IsGoogleAuthEnabled)
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                bool result = tfa.ValidateTwoFactorPIN(user.GoogleAuthKey, googleAuthCode);
                if (!result)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Failed to authenticate with Google Authenticator code. Please try again!" });
                }
            }

            return true;
        }
    }
}