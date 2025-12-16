using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Update;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequestDto, UpdateUserResponseDto>
{
    private readonly LearnContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UpdateUserHandler(LearnContext context, IPasswordHasher passwordHasher, ICurrentUserAccessor currentUserAccessor, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _currentUserAccessor = currentUserAccessor;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<UpdateUserResponseDto> Handle(UpdateUserRequestDto message, CancellationToken cancellationToken)
    {
        var currentEmail = _currentUserAccessor.GetCurrentEmail();
        var user = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Where(x => x.Email == currentEmail && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);
        user.Email = message.Email ?? user.Email;
        user.Username = message.Username ?? user.Username;

        
        if (message.BlobId != Guid.Empty || message.BlobId != null)
        {
            user.BlobId = message.BlobId;
        }
        if (!string.IsNullOrWhiteSpace(message.Password))
        {
            var salt = Guid.NewGuid().ToByteArray();
            user.Hash = _passwordHasher.Hash(message.Password, salt);
            user.Salt = salt;
        }
        TFAHelper.TwoFactorAuthentication(user, message.GoogleAuthCode);
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        var roles = string.Empty;
        foreach (var role in user.UserRoles)
        {
            roles += role.Role.RoleName + ", ";    
        }
        
        var userToken = await _jwtTokenGenerator.CreateToken(user.Email,roles, 120);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(userToken);

        return new UpdateUserResponseDto()
        {
            Email = user.Email,
            Token = userToken,
            TokenValidTo = token.ValidTo,
            Username = user.Username,
            BlobId = user.BlobId,
        };
    }
}