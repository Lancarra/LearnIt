using MediatR;

namespace Application.Users._2FaUth.CheckIfEnabled
{
    public class Check2FactorRequestDto : IRequest<bool>
    {
        public string Email { get; set; }
    }
}
