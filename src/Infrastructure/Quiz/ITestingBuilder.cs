using Domain.Models;
using Domain.Models.Quiz;

namespace Infrastructure.Quiz;

public interface ITestingBuilder
{
    public Task<TestCard> CreateQuiz(List<TestUnitRequestDto> request, CancellationToken cancellationToken);
    Task<TestCard> UpdateQuiz(Guid cardId, List<UpdateTestUnitViewModel> testUnits, CancellationToken cancellationToken);

}