using API.Controllers.Users.Constants;
using Application.StaticImages.Get;
using Application.Users.GetById;
using Infrastructure.BlobStorage.Entity;
using Infrastructure.BlobStorage.Service;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Blob;

public class BlobController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBlobService _blobService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BlobController(IMediator mediator, IBlobService blobService, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _blobService = blobService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [Authorize]
    [HttpGet("get-icons")]
    public async Task<IActionResult> Get()
    {
        var response = new List<dynamic>();
        var icons = await _mediator.Send(new GetStaticImagesRequest());
        foreach (var icon in icons.Images)
        {
            try
            {
                var result = await _blobService.DownloadAsync(icon, Constants.USER_CONTAINER);
                if (result == null || result.stream == null)
                {
                    return NotFound("Image not found");
                }
                response.Add(File(result.stream, result.contentType));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        return Ok(response);
    }
}