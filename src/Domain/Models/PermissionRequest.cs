namespace Domain.Models;

public class PermissionRequest
{
    public PermissionRequest()
    {
        RequestId = Guid.NewGuid();
    }
    public Guid RequestId { get; set; }
    public int RequestUserId { get; set; }
    public string RoleName { get; set; }

}