namespace Infrastructure.BlobStorage.Entity
{
    public record FileResponse(Stream stream, string contentType, string? url = null)
    {

    }
}
