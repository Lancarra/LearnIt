using MediatR;

namespace Application.Users.GetById;

public class GetUserByIdRequestDto : IRequest<GetUserByIdResponseDto>
{
    public int UserId { get; set; }

}