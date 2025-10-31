using Domain.Models;
using Domain.Models.Quiz;

namespace Infrastructure.Quiz;

public interface ITestingBuilder
{
    public Task<TestCard> CreateQuiz(string name, List<TestUnitRequestDto> request, CancellationToken cancellationToken);
    Task<TestCard> UpdateQuiz(Guid cardId, string name, List<UpdateTestUnitViewModel> testUnits, CancellationToken cancellationToken);

}