using Application.Folder.Create;
using Application.Folder.Delete;
using Application.Folder.GetAll;
using Application.LearnWordDictionary.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Folder;

[ApiController]
[Route("folder")]
public class FolderController: ControllerBase
{
    private readonly IMediator _mediator;
    public FolderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-all-folders")]
    public async Task<GetFolderResponseDto> GetFolder()
    {
        var folders = await _mediator.Send(new GetFolderRequestDto());
        return folders;
    }

    [HttpPost("create-folder")]
    public async Task<CreateFolderResponseDto> CreateFolder([FromBody] CreateFolderRequestDto request)
    {
        var folder = await _mediator.Send(request);
        return folder;
    }
    
    [HttpPut ("update-folder")]
    public async Task<UpdateDictionaryResponseDto> UpdateFolder([FromBody] UpdateDictionaryRequestDto request)
    {
        var folder = await _mediator.Send(request);
        return folder;
    }
    
    [HttpDelete("delete-folder")]
    public async Task<DeleteFolderResponseDto> DeleteFolder([FromBody] DeleteFolderRequestDto request)
    {
        var folder = await _mediator.Send(request);
        return folder;
    }
}
