using MediatR;

namespace Application.Users.Update;

public class UpdateUserRequestDto : IRequest<UpdateUserResponseDto>
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string GoogleAuthCode { get; set; }

    public Guid BlobId { get; set; }
}