using Application.CourseModule.Create;
using Application.Quiz;
using Application.Quiz.Answer.CheckAnswer;
using Application.Quiz.Answer.Create;
using Application.Quiz.Get;
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
    public async Task<CreateQuizAnswerResponseDto> CreatAnswerQuiz([FromBody] CreateQuizAnswerRequestDto request)
    {
        var result = await _mediator.Send(request);
        return result;
    }
}