using MediatR;

namespace Application.Users._2FaUth.Generate
{
    public class Generate2FAuthRequestDTO : IRequest<Generate2FAuthResponseDTO>
    {
        public bool Retry { get; set; }
        public string Password { get; set; }
    }
}
