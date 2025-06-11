using Application.LearnWordDictionary.Create;
using Application.LearnWordDictionary.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.LearnWordDictionary;

[ApiController]
[Route("LearnWordDictionary")]
public class DictionaryController : ControllerBase
{
    private readonly IMediator _mediator;
    public DictionaryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("CreateDictionary")]
    public async Task <CreateDictionaryResponseDto> CreateDictionary([FromBody] CreateDictionaryRequestDto request)
    {
        var dictionary = await _mediator.Send(request);
        return dictionary;
    }
    
    [HttpPut ("UpdateDictionary")]
    public async Task<UpdateDictionaryResponseDto> UpdateModule([FromBody] UpdateDictionaryRequestDto request)
    {
        var module = await _mediator.Send(request);
        return module;
    }
}
