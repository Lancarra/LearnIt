namespace Application.CourseModule.GetByTeacherId;

public class GetByTeacherIdResponseDto
{
    public IEnumerable<GetByTeacherIdViewModel> Modules { get; set; }
    public int Count { get; set; }
    public int StudentCount { get; set; }

    public GetByTeacherIdResponseDto(List<GetByTeacherIdViewModel> modules)
    {
        Modules = modules;
    }
}