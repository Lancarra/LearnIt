namespace Application.CourseModule.GetAll;

public class GetModuleResponseDto
{
    public IEnumerable<GetModuleViewModel> Modules { get; set; }

    public GetModuleResponseDto(List<GetModuleViewModel> modules)
    {
        Modules = modules;
    }
}