using System.Net;
using Domain;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Create;

public class CreateUserHandler : IRequestHandler<CreateUserRequestDto, CreateUserResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly IPasswordHasher _passwordHasher;
    
    public CreateUserHandler( IMediator mediator, LearnContext context, IPasswordHasher passwordHasher)
    {
        _mediator = mediator;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateUserResponseDto> Handle(CreateUserRequestDto request, CancellationToken cancellationToken)
    {
        if (await _context.Users.Where(x => x.Email == request.Email && !x.IsDeleted).AnyAsync(cancellationToken))
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Email = $"User with email {request.Email} already exists"});
        }

        var salt = Guid.NewGuid().ToByteArray();
        {
            var person = new User()
            {
                Email = request.Email,
                Username = request.Username,
                Hash = _passwordHasher.Hash(request.Password, salt),
                Salt = salt
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