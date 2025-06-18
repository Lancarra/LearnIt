using Application.Folder.Create;
using Application.Folder.Delete;
using Application.Folder.GetAll;
using Application.LearnWordDictionary.Update;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Folder;

[Authorize]
[ApiController]
[Route("folder")]
public class FolderController: ControllerBase
{
    private readonly IMediator _mediator;
    public FolderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-all-folders/{courseModuleId}")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<GetFolderResponseDto> GetFolder([FromRoute] Guid courseModuleId)
    {
        var folders = await _mediator.Send(new GetFolderRequestDto
        {
            CourseModuleId = courseModuleId
        });
        return folders;
    }

    [HttpPost("create-folder")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<CreateFolderResponseDto> CreateFolder([FromBody] CreateFolderRequestDto request)
    {
        var folder = await _mediator.Send(request);
        return folder;
    }
    
    [HttpPut ("update-folder")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<UpdateDictionaryResponseDto> UpdateFolder([FromBody] UpdateDictionaryRequestDto request)
    {
        var folder = await _mediator.Send(request);
        return folder;
    }
    
    [HttpDelete("delete-folder")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<DeleteFolderResponseDto> DeleteFolder([FromBody] DeleteFolderRequestDto request)
    {
        var folder = await _mediator.Send(request);
        return folder;
    }
}
