using System.Net;
using Application.Users.UpdatePermission;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.LearnWordDictionary.Update;

public class UpdateDictionaryHandler : IRequestHandler<UpdateDictionaryRequestDto, UpdateDictionaryResponseDto>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    
    public UpdateDictionaryHandler(IMediator mediator, LearnContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _mediator = mediator;
        _context = context;
        _currentUserAccessor = currentUserAccessor; 
    }

    public async Task<UpdateDictionaryResponseDto> Handle(UpdateDictionaryRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _currentUserAccessor.GetCurrentEmail() 
                                      && !x.IsDeleted, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var folderExists = await _context.Folders.AnyAsync(f => f.Id == request.ParentFolderId, cancellationToken);
        if (!folderExists)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {ParentFolderId = $"Folder with id {request.ParentFolderId} does not exist"});
        }
        
        var dictionary = _context.LearnWordDictionaries.FirstOrDefault(lwd => lwd.Id == request.Id);
        if (dictionary == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new {Id = $"Dictionary with id {request.Id} does not exist"});
        }
        
        var dictionaryDublicate = await _context.LearnWordDictionaries.AnyAsync(lwd => lwd.Name == request.Name && lwd.Id != request.Id, cancellationToken);
        if (dictionaryDublicate)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Name = $"Dictionary with name {request.Name} already exists"});
        }
        
        dictionary.Name = request.Name;
        dictionary.ParentFolderId = request.ParentFolderId;
        if (request.FinishStudy)
        {
            user.Rating += 5;
            await UserRatingUpdating.UpdateAchievement(user, _context, cancellationToken);
        }
        await _context.SaveChangesAsync(cancellationToken);
        
        return new UpdateDictionaryResponseDto()
        {
            Id = dictionary.Id,
            Name = dictionary.Name,
            ParentFolderId = dictionary.ParentFolderId
        };
    }
}