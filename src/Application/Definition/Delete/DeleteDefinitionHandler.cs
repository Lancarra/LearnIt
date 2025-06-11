using System.Net;
using Application.Users.Create;
using Domain;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Definition.Delete;

public class CreateUserHandler : IRequestHandler<CreateUserRequestDto, CreateUserResponseDto>
{
    private readonly LearnContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMediator _mediator;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public CreateUserHandler(LearnContext context, IPasswordHasher passwordHasher, IMediator mediator, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _mediator = mediator;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<CreateUserResponseDto> Handle(CreateUserRequestDto message,
        CancellationToken cancellationToken)
    {
        if (await _context.Users.Where(x => x.Email == message.Email && !x.IsDeleted).AnyAsync(cancellationToken))
        {
            throw new RestException(HttpStatusCode.BadRequest, new { Email = "User with this email already exists!" });
        }

        var salt = Guid.NewGuid().ToByteArray();

        {
            var person = new User()
            {
                Email = message.Email,
                Hash = _passwordHasher.Hash(message.Password, salt),
                Salt = salt,
            };

            _context.Users.Add(person);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateUserResponseDto()
            {
                UserId = person.UserId
            };
        }
    }
}