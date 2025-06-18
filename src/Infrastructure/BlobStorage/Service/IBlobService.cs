using Infrastructure.BlobStorage.Entity;

namespace Infrastructure.BlobStorage.Service
{
    public interface IBlobService
    {
        Task<Guid> UploadAsync(Stream stream, string contentType, string containerName, CancellationToken cancellationToken = default);
        Task<FileResponse> DownloadAsync(Guid fileId, string containerName, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid fileId, string containerName, CancellationToken cancellationToken = default);
        Task<List<FileResponse>> DownloadStaticIconsAsync();
    }
}
