using Application.Folder.Create;
using Application.LearnWordDictionary.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Folder;

[ApiController]
[Route("Folder")]
public class FolderController: ControllerBase
{
    private readonly IMediator _mediator;
    public FolderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateFolder")]
    public async Task<CreateFolderResponseDto> CreateFolder([FromBody] CreateFolderRequestDto request)
    {
        var folder = await _mediator.Send(request);
        return folder;
    }
    
    [HttpPut ("UpdateFolder")]
    public async Task<UpdateDictionaryResponseDto> UpdateFolder([FromBody] UpdateDictionaryRequestDto request)
    {
        var folder = await _mediator.Send(request);
        return folder;
    }
}
