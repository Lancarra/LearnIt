using MediatR;

namespace Application.Users.Delete;

public class DeleteUserRequestDto : IRequest<bool>
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public Guid BlobId { get; set; }
}