using MediatR;

namespace Application.Users._2FaUth.Verify
{
    public class Verify2FAuthRequestDTO : IRequest<Unit>
    {
        public string GoogleCode { get; set; }
    }
}
