using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.BlobStorage.Entity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.BlobStorage.Service
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly IConfiguration _configuration;
        public BlobService(BlobServiceClient BlobServiceClient, IConfiguration configuration)
        {
            blobServiceClient = BlobServiceClient;
            _configuration = configuration;
        }

        public async Task<Guid> UploadAsync(Stream stream, string contentType, string containerName, CancellationToken cancellationToken = default)
        {
            var blobSettings = _configuration.GetSection("BlobSettings");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);
            var fileId = Guid.NewGuid();
            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());
            await blobClient.UploadAsync(stream, new BlobHttpHeaders() { ContentType = contentType },
                cancellationToken: cancellationToken);
            return fileId;
        }

        public async Task<FileResponse> DownloadAsync(Guid fileId, string containerName, CancellationToken cancellationToken = default)
        {
            var blobSettings = _configuration.GetSection("BlobSettings");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());
            Response<BlobDownloadResult> response = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);
            return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
        }

        public async Task DeleteAsync(Guid fileId, string containerName, CancellationToken cancellationToken = default)
        {
            var blobSettings = _configuration.GetSection("BlobSettings");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        }

        public async Task<List<FileResponse>> DownloadStaticIconsAsync()
        {
            List<FileResponse> fileResponses = new List<FileResponse>();

            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetSection("BlobSettings")["BlobConnection"]);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("static-icons");

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);

                MemoryStream memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;

                string contentType = blobItem.Properties.ContentType ?? "application/octet-stream";

                fileResponses.Add(new FileResponse(memoryStream, contentType, blobClient.Uri.ToString()));
            }
            return fileResponses;
        }
    }
}
