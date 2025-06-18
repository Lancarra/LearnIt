using MediatR;

namespace Application.Users.GetById;

public class GetUserRequestDto : IRequest<GetUserResponseDto>
{
    public int UserId { get; set; }

}