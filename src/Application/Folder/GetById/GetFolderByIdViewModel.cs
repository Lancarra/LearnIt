namespace Application.Folder.GetById;

public class GetFolderByIdViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid CourseModuleId { get; set; }
    public IEnumerable<DictionaryViewModel> Dictionaries { get; set; } = new List<DictionaryViewModel>();
}

public class DictionaryViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
}