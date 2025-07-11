namespace API.Controllers.Definition;

public class UpdateDefinitionRequest
{
    public Guid DefintionId { get; set; }
    public string ImageUrl { get; set; }
    public bool SaveToBlob { get; set; }
}