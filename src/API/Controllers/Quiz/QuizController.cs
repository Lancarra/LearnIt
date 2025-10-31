using Application.CourseModule.Create;
using Application.Quiz;
using Application.Quiz.Answer.CheckAnswer;
using Application.Quiz.Answer.CheckSingleAnswer;
using Application.Quiz.Answer.Create;
using Application.Quiz.Get;
using Application.Quiz.GetCards;
using Application.Quiz.Update;
using Infrastructure.BlobStorage.Service;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Quiz;

[Authorize]
[ApiController]
[Route("quiz")]
public class QuizController
{
    private readonly IMediator _mediator;
    public QuizController(IMediator mediator, IBlobService blobService)
    {
        _mediator = mediator;
    }
    
    [HttpPost("create-quiz")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<CreateQuizResponseDto> CreateQuiz([FromBody] CreateQuizRequestDto request)
    {
        var quiz = await _mediator.Send(request);
        return quiz;
    }

    [HttpGet("get-quiz/{cardId}")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<GetQuizResponseDto> GetQuiz([FromRoute] Guid cardId)
    {
        var quiz = await _mediator.Send(new GetQuizRequestDto(){TestCardId =  cardId});
        return quiz;
    }
    
    [HttpGet("get-quiz-by-dictionary-id/{dictionaryId}")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<GetAllTestCardByDictionaryIdResponseDto> GetCards([FromRoute] Guid dictionaryId)
    {
        var result = await _mediator.Send(new GetAllTestCardByDictionaryIdRequestDto(){DictionaryId = dictionaryId});
        return result;
    }

    [HttpPatch("update-quiz")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<UpdateQuizResponseDto> UpdateQuiz([FromBody] UpdateQuizRequestDto request)
    {
        var quiz = await _mediator.Send(request);
        return quiz;
    }
    
    [HttpPost("check-quiz")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<CheckAnswerResponseDto> CheckQuiz([FromBody] CheckAnswerRequestDto request)
    {
        var result = await _mediator.Send(request);
        return result;
    }
    
    [HttpPost("create-answer-quiz")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<CreateQuizAnswerResponseDto> CreateAnswerQuiz([FromBody] CreateQuizAnswerRequestDto request)
    {
        var result = await _mediator.Send(request);
        return result;
    }
    
    [HttpPost("check-answer")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<CheckSingleAnswerResponseDto> CheckAnswer([FromBody] CheckSingleAnswerRequestDto request)
    {
        var result = await _mediator.Send(request);
        return result;
    }
}