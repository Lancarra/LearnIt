using System.Text.Json.Serialization;

namespace Domain.Models;

public class LearnWordDictionary
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Definition> Definitions { get; set; }
    public virtual ICollection<TestCard> TestCards { get; set; }
    public virtual Folder ParentFolder { get; set; }
    public Guid ParentFolderId { get; set; }
}