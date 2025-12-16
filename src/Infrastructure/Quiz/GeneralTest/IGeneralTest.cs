using Domain.Models;
using Domain.Models.Quiz;

namespace Infrastructure.Quiz.GeneralTest;

public interface IGeneralTest
{
    public Task<TestCard> CreateGeneralTest(string name, Guid moduleId, int definitionCount, CancellationToken cancellationToken);

}