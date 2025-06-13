using Application.LearnWordDictionary.Create;
using Application.LearnWordDictionary.Delete;
using Application.LearnWordDictionary.GetAll;
using Application.LearnWordDictionary.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.LearnWordDictionary;

[ApiController]
[Route("learn-word-dictionary")]
public class DictionaryController : ControllerBase
{
    private readonly IMediator _mediator;
    public DictionaryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("get-all-dictionaries")]
    public async Task<GetDictionaryResponseDto> GetDictionary()
    {
        var dictionaries = await _mediator.Send(new GetDictionaryRequestDto());
        return dictionaries;
    }
    
    [HttpPost("create-dictionary")]
    public async Task <CreateDictionaryResponseDto> CreateDictionary([FromBody] CreateDictionaryRequestDto request)
    {
        var dictionary = await _mediator.Send(request);
        return dictionary;
    }
    
    [HttpPut ("update-dictionary")]
    public async Task<UpdateDictionaryResponseDto> UpdateDictionary([FromBody] UpdateDictionaryRequestDto request)
    {
        var dictionary = await _mediator.Send(request);
        return dictionary;
    }
    
    [HttpDelete("delete-dictionary")]
    public async Task<DeleteDictionaryResponseDto> DeleteDictionary([FromBody] DeleteDictionaryRequestDto request)
    {
        var dictionary = await _mediator.Send(request);
        return dictionary;
    }
}
