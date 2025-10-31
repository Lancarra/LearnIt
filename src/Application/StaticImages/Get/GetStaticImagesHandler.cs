using System.Net.Mime;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.StaticImages.Get;

public class GetStaticImagesHandler : IRequestHandler<GetStaticImagesRequest, GetStaticImagesResponse>
{
    private readonly IMediator _mediator;
    private readonly LearnContext _context;

     
    public GetStaticImagesHandler( IMediator mediator, LearnContext context)
    {
        _mediator = mediator;
        _context = context;
    }
    
    public async Task<GetStaticImagesResponse> Handle(GetStaticImagesRequest request, CancellationToken cancellationToken)
    {
        var icons = await _context.StaticImages.ToListAsync(cancellationToken);
        var response = new GetStaticImagesResponse()
        {
            Images = new List<Guid>()
        };
        foreach (var icon in icons)
        {
            response.Images.Add(icon.FileBlobId);
        }
        return response;
    }
}