using Application.Users.Login.ServiceLoginRequests;
using Application.Users.Login.ServiceLoginResponses;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Users.Create;

namespace Application.Users.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserRequestDto, LoginUserResponseDto>
    {
        private readonly LearnContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public LoginUserHandler(LearnContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IConfiguration configuration, IMediator mediator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<LoginUserResponseDto> Handle(LoginUserRequestDto message, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(x => x.Email.Equals(message.Email) && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);
            if (user == null && !message.FromOtherService)
            {
                throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
            }
            else if (user == null && message.FromOtherService && message.VerificationString == _configuration["VerificationRandomString"])
            {
                await _mediator.Send(new CreateUserRequestDto()
                {
                    Email = message.Email,
                    Password = message.Password,
                });
                user = await _context.Users.Where(x => x.Email.Equals(message.Email) && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);
                if (message.GoogleAuthCode != null && message.GoogleAuthCode != "" && message.GoogleAuthCode != "string")
                {
                    user.GoogleAuthKey = message.GoogleAuthKey;
                    user.IsGoogleAuthEnabled = true;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            if (!user.Hash.SequenceEqual(_passwordHasher.Hash(message.Password, user.Salt)))
            {
                throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
            }

            TFAHelper.TwoFactorAuthentication(user, message.GoogleAuthCode);

            var LearnItToken = "";

            if (!message.FromOtherService)
            {
                using HttpClient client = new HttpClient();
                if (_configuration["IsDockerized"] == "true")
                {
                    client.BaseAddress = new Uri(_configuration["DockerizedLearnItBackendUrl"] ?? "http://host.docker.internal:5000/");
                }
                else
                {
                    client.BaseAddress = new Uri(_configuration["LearnItBackendUrl"] ?? "http://localhost:5000/");
                }

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                LearnItResponse loginResponse = null;
                LearnItRequest loginRequest = new LearnItRequest()
                {
                    User = new LearnItRequest.UserRequest()
                    {
                        Email = message.Email,
                        FromAnotherService = true,
                        GoogleAuthKey = message.GoogleAuthCode,
                        Password = message.Password
                    }
                };
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "users/login",
                        loginRequest,
                        cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        // Replace this line:
                        // loginResponse = await response.Content.ReadAsAsync<LearnItResponse>(cancellationToken);
                        // With the following:
                        loginResponse = await response.Content.ReadFromJsonAsync<LearnItResponse>(cancellationToken: cancellationToken);
                        LearnItToken = loginResponse.User.Token;
                    }
                }
                catch (Exception e) 
                {
                    LearnItToken = "Could not connect to the LearnIt Backend API!";
                }

            }

            var userToken = await _jwtTokenGenerator.CreateToken(user.Email, 120);
            var token = new JwtSecurityTokenHandler().ReadJwtToken(userToken);

            return new LoginUserResponseDto()
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = userToken,
                LearnItToken = LearnItToken,
                TokenValidTo = token.ValidTo,
                Has2FAuthEnabled = user.IsGoogleAuthEnabled,
                //BlobId = (Guid)user.BlobId,
            };
        }
    }
}
