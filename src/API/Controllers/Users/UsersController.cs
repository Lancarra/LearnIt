using Application.Users._2FaUth.CheckIfEnabled;
using Application.Users.Create;
using Application.Users.Delete;
using Application.Users.GetById;
using Application.Users.Login;
using Application.Users.Update;
using Infrastructure.BlobStorage.Service;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers.Users;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBlobService _blobService;
    public UsersController(IMediator mediator, IBlobService blobService)
    {
        _mediator = mediator;
        _blobService = blobService;
    }

    [HttpGet("get-user-by-id/{userId:int}")]
    public async Task<GetUserResponseDto> Get([FromRoute] int userId)
    {
        var user = await _mediator.Send(new GetUserRequestDto { UserId = userId });
        return user;
    }
    
    [HttpPost("create-user")]
    public async Task<CreateUserResponseDto> Create([FromBody] CreateUserRequestDto request , IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var blobId = await _blobService.UploadAsync(stream, file.ContentType, Constants.Constants.CONTAINER); 
        request.BlobId = blobId;

        var response = await _mediator.Send(request);

        return response;
    } 
    
    [HttpGet("check2factor/{email}")]
    public async Task<bool> Check2Factor([FromRoute] string email)
    {
        return await _mediator.Send(new Check2FactorRequestDto() { Email = email });
    }
    
    [HttpPost("login")]
    public async Task<LoginUserResponseDto> Login([FromBody] LoginUserRequestDto request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpPut("update-user")]
    public async Task<UpdateUserResponseDto> Update([FromBody] UpdateUserRequestDto request, IFormFile file)
    {
        if (request.BlobId != null)
        {
            await _blobService.DeleteAsync(request.BlobId, Constants.Constants.CONTAINER);

            await using var stream = file.OpenReadStream();
            var blobId = await _blobService.UploadAsync(stream, file.ContentType, Constants.Constants.CONTAINER);
            request.BlobId = blobId;
        }
        return await _mediator.Send(request);
    }

    [HttpDelete("delete-user")]
    public async Task<bool> Delete([FromBody] DeleteUserRequestDto request)
    {
        var response = await _mediator.Send(request);

        await _blobService.DeleteAsync(request.BlobId, Constants.Constants.CONTAINER);

        return response;
    }

    [HttpPut("blob/get/{fileBlobId:guid}")]
    public async Task<IActionResult> DownloadFileBlob(Guid fileBlobId)
    {
        try
        {
            var result = await _blobService.DownloadAsync(fileBlobId, Constants.Constants.CONTAINER);
            if (result == null || result.stream == null)
            {
                return NotFound("Image not found");
            }
            return File(result.stream, result.contentType);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}