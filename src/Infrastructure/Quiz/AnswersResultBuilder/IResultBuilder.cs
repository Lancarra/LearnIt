using Domain.Models.Quiz;

namespace Infrastructure.Quiz.AnswersResultBuilder;

public interface IResultBuilder
{
    public Task<ResultBuilderModel> GetQuizResult(ResultBuilderRequest request, CancellationToken cancellationToken);
}