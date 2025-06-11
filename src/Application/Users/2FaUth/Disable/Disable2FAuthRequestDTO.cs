using MediatR;

namespace Application.Users._2FaUth.Disable
{
    public class Disable2FAuthRequestDTO : IRequest<Unit>
    {
        public string Password { get; set; }
        public string GoogleCode { get; set; }
    }
}
