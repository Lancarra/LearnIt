using System.Net;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Database;
using Infrastructure.Errors;
using Infrastructure.Helpers;
using Infrastructure.Quiz.AnswersResultBuilder;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Quiz.ResultByDictionaryKakoito;

public class ResultByDictionaryHandler : IRequestHandler<ResultByDictionaryRequestDto, ResultByDictionaryResponseDto>
{
    private readonly IResultBuilder _builder;
    private readonly ICurrentUserAccessor _userAccessor;
    private LearnContext _context;
    
    public ResultByDictionaryHandler(IResultBuilder builder, ICurrentUserAccessor userAccessor, LearnContext context)
    {
        _builder = builder;
        _userAccessor = userAccessor;
        _context = context;
    }
    public async Task<ResultByDictionaryResponseDto> Handle(ResultByDictionaryRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Email == _userAccessor.GetCurrentEmail() 
                                      && !x.IsDeleted, cancellationToken);
        PropertyChecker.CheckNullAndThrow404(user);
        
        var isAdmin = user.UserRoles.Any(ur => ur.Role.RoleName == "Admin");
        
        var response = new ResultByDictionaryResponseDto(){Result = new List<ResultByDictionaryViewModel>()};
        var students = await _context.Users.Where(u => u.TeachersId.Contains(request.TeacherId)).ToListAsync(cancellationToken);
        var cards = await _context.TestCards.Include(c => c.TestCardAnswers)
            .Where(tc => tc.DictionaryId == request.DictionaryId) 
            .ToListAsync(cancellationToken);
        foreach (var card in cards)
        {
            foreach (var answer in card.TestCardAnswers)
            {
                try
                {
                    if (students.Any(s => s.UserId == answer.UserId) || isAdmin)
                    {
                        var result = await _builder.GetQuizResult(new ResultBuilderRequest
                        {
                            CardId = card.Id,
                            CardAnswerId = answer.Id,
                        }, cancellationToken);

                        if (!isAdmin)
                        {
                            response.Result.Add(new ResultByDictionaryViewModel
                            {
                                TestName = card.Name,
                                UserName = students.FirstOrDefault(u => u.UserId == answer.UserId).Username,
                                CorrectAnswers = result.CorrectAnswers,
                                IncorrectAnswers = result.IncorrectAnswers,
                            });
                        }
                        else
                        {
                            var userName = await _context.Users.FirstOrDefaultAsync(u => u.UserId == answer.UserId);
                            response.Result.Add(new ResultByDictionaryViewModel
                            {
                                TestName = card.Name,
                                UserName = userName.Username,
                                CorrectAnswers = result.CorrectAnswers,
                                IncorrectAnswers = result.IncorrectAnswers,
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new {ex.Message});
                }
            }
        }
        
        return response;
    }
}