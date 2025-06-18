using Infrastructure.CurrentUserAccessor;
using MediatR;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.Users._2FaUth.CheckIfEnabled
{
    public class Check2FactorHandler : IRequestHandler<Check2FactorRequestDto, bool>
    {
        private readonly LearnContext _context;
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public Check2FactorHandler(LearnContext context, IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<bool> Handle(Check2FactorRequestDto message, CancellationToken cancellationToken)
        {
            var person = await _context.Users.Where(x => x.Email.Equals(message.Email) && !x.IsDeleted).SingleOrDefaultAsync(cancellationToken);
            if (person == null)
            {
                return false;
            }

            return person.IsGoogleAuthEnabled;


        }
    }
}
