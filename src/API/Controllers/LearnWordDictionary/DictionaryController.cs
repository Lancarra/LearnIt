using Application.LearnWordDictionary.Create;
using Application.LearnWordDictionary.Delete;
using Application.LearnWordDictionary.GetAll;
using Application.LearnWordDictionary.Update;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.LearnWordDictionary;

[Authorize]
[ApiController]
[Route("learn-word-dictionary")]
public class DictionaryController : ControllerBase
{
    private readonly IMediator _mediator;
    public DictionaryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("get-all-dictionaries/{parentFolderId}")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<GetDictionaryResponseDto> GetDictionary([FromRoute] Guid parentFolderId)
    {
        var dictionaries = await _mediator.Send(new GetDictionaryRequestDto
        {
            ParentFolderId = parentFolderId
        });
        return dictionaries;
    }
    
    [HttpPost("create-dictionary")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task <CreateDictionaryResponseDto> CreateDictionary([FromBody] CreateDictionaryRequestDto request)
    {
        var dictionary = await _mediator.Send(request);
        return dictionary;
    }
    
    [HttpPut ("update-dictionary")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<UpdateDictionaryResponseDto> UpdateDictionary([FromBody] UpdateDictionaryRequestDto request)
    {
        var dictionary = await _mediator.Send(request);
        return dictionary;
    }
    
    [HttpDelete("delete-dictionary")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<DeleteDictionaryResponseDto> DeleteDictionary([FromBody] DeleteDictionaryRequestDto request)
    {
        var dictionary = await _mediator.Send(request);
        return dictionary;
    }
}
