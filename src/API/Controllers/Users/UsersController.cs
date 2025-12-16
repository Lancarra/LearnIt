using Application.PermissionRequests;
using Application.Users._2FaUth.CheckIfEnabled;
using Application.Users.Create;
using Application.Users.Delete;
using Application.Users.GetAll;
using Application.Users.GetAllStudentsByTeacherId;
using Application.Users.GetById;
using Application.Users.GetRole;
using Application.Users.GetStudents;
using Application.Users.GetTeachers;
using Application.Users.GetUserRole;
using Application.Users.Login;
using Application.Users.Update;
using Application.Users.UpdatePermission;
using Application.Users.UpdateRelation.UpdateTeacher;
using Azure;
using Azure.Core;
using Infrastructure.BlobStorage.Service;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers.Users;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBlobService _blobService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UsersController(IMediator mediator,IBlobService blobService, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _blobService = blobService;
        _httpContextAccessor = httpContextAccessor;
    }
    [Authorize]
    [HttpGet("get-users")]
    public async Task<GetAllResponseDto> GetUsers()
    {
        var users = await _mediator.Send(new GetAllRequestDto());
        return users;
    }
    
    [Authorize]
    [HttpGet("get-user")]
    public async Task<GetUserResponseDto> Get()
    {
        var user = await _mediator.Send(new GetUserRequestDto { UserId = 0 });
        return user;
    }
    
    [Authorize]
    [HttpGet("get-user/{userId:int}")]
    public async Task<GetUserByIdResponseDto> GetUserById([FromRoute] int userId)
    {
        var userById = await _mediator.Send(new GetUserByIdRequestDto { UserId = userId });
        return userById;
    }
    
    [HttpPost("create-user")]
    public async Task<CreateUserResponseDto> Create([FromBody] CreateUserRequestDto request )
    {
        return await _mediator.Send(request);
    } 
    
    [Authorize]
    [HttpGet("check2factor/{email}")]
    public async Task<bool> Check2Factor([FromRoute] string email)
    {
        return await _mediator.Send(new Check2FactorRequestDto() { Email = email });
    }
    
    [HttpPost("login")]
    public async Task<LoginUserResponseDto> Login([FromBody] LoginUserRequestDto request)
    {
        return await _mediator.Send(request);
    }
    [Authorize]
    [HttpPost("logout/{userId:int}")]
    public async Task<IActionResult> Logout([FromRoute] int userId, CancellationToken cancellationToken)
    {
        _httpContextAccessor.HttpContext.Session.Remove(userId.ToString());
        _httpContextAccessor.HttpContext?.Abort();
        return Ok(new { Success = true });
    }
    
    [Authorize]
    [HttpPut("update-user")]
    public async Task<UpdateUserResponseDto> Update([FromBody] UpdateUserRequestDto request)
    {
        return await _mediator.Send(request);
    }

    [Authorize]
    [HttpDelete("delete-user")]
    public async Task<bool> Delete([FromBody] DeleteUserRequestDto request)
    {
        var response = await _mediator.Send(request);

        await _blobService.DeleteAsync(request.BlobId, Constants.Constants.USER_CONTAINER);

        return response;
    }

    [Authorize]
    [HttpGet("get-user-role/{userId:int}")]
    public async Task<GetUserRoleResponseDto> GetUserRole([FromRoute] int userId)
    {
        return await _mediator.Send(new GetUserRoleRequestDto { UserId = userId });
    }

    [Authorize]
    [HttpGet("get-roles")]
    public async Task<GetRoleResponseDto> GetRoles()
    {
        return await _mediator.Send(new GetRoleRequestDto());
    }
    
    [Authorize]
    [HttpPost("update-users-permissions")]
    public async Task<UpdatePermissionResponseDto> UpdatePermission([FromBody] UpdatePermissionRequestDto request)
    {
       return await _mediator.Send(request);
    }

    [Authorize]
    [HttpGet("blob/get/{fileBlobId:guid}")]
    public async Task<IActionResult> DownloadFileBlob(Guid fileBlobId)
    {
        try
        {
            var result = await _blobService.DownloadAsync(fileBlobId, Constants.Constants.USER_CONTAINER);
            if (result == null || result.stream == null)
            {
                return NotFound("Image not found");
            }
            return File(result.stream, result.contentType);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpPost("blob/update/{userId:int}")]
    public async Task<IActionResult> UpdateDocumentFile([FromRoute] int userId, IFormFile file)
    {
        Guid blobId = Guid.Empty;

        try
        {
            var response = await _mediator.Send(new GetUserRequestDto { UserId = userId });

            if (response.BlobId != Guid.Empty && response.BlobId != null)
            {
                await _blobService.DeleteAsync((Guid)response.BlobId, Constants.Constants.USER_CONTAINER);
            }
            await using var stream = file.OpenReadStream(); 
            blobId = await _blobService.UploadAsync(stream, file.ContentType, Constants.Constants.USER_CONTAINER);

            var updateResponse = await _mediator.Send(new UpdateUserRequestDto
            {
                Email = response.Email,
                BlobId = blobId
            });
            return Ok(updateResponse);
        }
        catch (Exception exception)
        {
            await _blobService.DeleteAsync(blobId, Constants.Constants.USER_CONTAINER);
            return BadRequest(exception.Message);
        }
    }
    
    
    [Authorize]
    [HttpPut("assign-student")]
    public async Task<UpdateTeacherResponseDto> AssignStudent([FromBody] UpdateTeacherRequestDto request)
    {
        return await _mediator.Send(request);
    }
    
    [Authorize]
    [HttpGet("get-students/{teacherId:int}")]
    public async Task<GetAllStudentsByTeacherIdResponseDto> GetStudents([FromRoute]  int teacherId)
    {
        var students = await _mediator.Send(new GetAllStudentsByTeacherIdRequestDto(){TeacherId = teacherId});
        return students;
    }
    
    [Authorize]
    [HttpGet("/admin/get-teachers")]
    public async Task<GetTeachersResponseDto> GetTeachers()
    {
        var teachers = await _mediator.Send(new  GetTeachersRequestDto());
        return teachers;
    }
    
    [Authorize]
    [HttpGet("/admin/get-students")]
    public async Task<GetStudentsResponseDto> GetStudents()
    {
        var students = await _mediator.Send(new  GetStudentsRequestDto());
        return students;
    }

    [Authorize]
    [HttpPost("permission-request")]
    public async Task<PermissionRequestsResponseDto> PermissionRequest([FromBody] PermissionRequestsRequestDto request, CancellationToken cancellationToken)
    {
        var permissions = await _mediator.Send(request);
        return permissions;
    }
}