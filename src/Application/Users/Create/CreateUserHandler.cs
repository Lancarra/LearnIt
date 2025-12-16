using System.Net;
using Application.Users.Login;
using Domain;
using Domain.Models;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
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
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == request.RoleName, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(role);
        var salt = Guid.NewGuid().ToByteArray();
        try
        {
            var person = new User()
            {
                Email = request.Email,
                Username = request.Username,
                Hash = _passwordHasher.Hash(request.Password, salt),
                Salt = salt,
                //BlobId = request.BlobId,111111
            };
            await _context.Users.AddAsync(person, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        var personCreated = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken); 
        await _context.UserRole.AddAsync(new UserRole
        {
            UserId = personCreated.UserId,
            User = personCreated,
            RoleId = role.RoleId,
            Role = role
        }, cancellationToken);
            
        await _context.SaveChangesAsync(cancellationToken);
        var loginResponse = await _mediator.Send(new LoginUserRequestDto
        {
            Email = personCreated.Email,
            Password = request.Password,
        }, cancellationToken);
        return new CreateUserResponseDto()
        {
            UserId = personCreated.UserId,
            RoleName = role.RoleName,
            Response = loginResponse
        };
    }
}