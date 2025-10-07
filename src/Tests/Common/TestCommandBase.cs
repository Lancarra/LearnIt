using Infrastructure.Database;

namespace Tests.Common;

public abstract class TestCommandBase : IDisposable
{
    protected readonly LearnContext Context;

    public TestCommandBase()
    {
        Context = LearnItContextFactory.Create();       
    }
    
    public void Dispose()
    {
        LearnItContextFactory.Destroy(Context);       
    }
}