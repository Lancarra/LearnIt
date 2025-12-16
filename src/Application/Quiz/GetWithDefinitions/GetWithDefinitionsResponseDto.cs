namespace Application.Quiz.GetWithDefinitions;

public class GetWithDefinitionsResponseDto 
{
    public Guid CardId { get; set; }
    public string Name { get; set; }
    public ICollection<GetWithDefinitionsViewModel> TestUnits { get; set; } = new List<GetWithDefinitionsViewModel>();
    public int DefinitionsCount { get; set; }
    
}