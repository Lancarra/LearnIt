using MediatR;

namespace Infrastructure.Database
{
    public class LearnItContextPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly LearnContext _context;

        public LearnItContextPipelineBehaviour(LearnContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse result;

            try
            {
                await _context.BeginTransactionAsync(cancellationToken);

                result = await next();

                await _context.CommitTransaction(cancellationToken);
            }
            catch (Exception)
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return result;
        }
    }
}
