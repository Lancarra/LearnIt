using System.Net;
using Domain.Models;
using Infrastructure.Database;
using Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.PermissionRequests;

public class PermissionRequestsHandler : IRequestHandler<PermissionRequestsRequestDto, PermissionRequestsResponseDto>
{
    private readonly LearnContext _context;
    
    public PermissionRequestsHandler(LearnContext context)
    {
        _context = context;
    }
    
    public async Task<PermissionRequestsResponseDto> Handle(PermissionRequestsRequestDto request, CancellationToken cancellationToken)
    {
        var requestEntity = await _context.PermissionRequests.FirstOrDefaultAsync(pr => pr.RequestUserId == request.RequestUserId && pr.RoleName == request.RoleName);
        if (requestEntity != null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new {Name = $"Permission request already exists"});
        }
        var permission = new PermissionRequest()
        {
            RequestUserId = request.RequestUserId,
            RoleName = request.RoleName,
        };

        await _context.PermissionRequests.AddAsync(permission, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new PermissionRequestsResponseDto()
        {
            Response = "Permission request created",
        };
    }
}