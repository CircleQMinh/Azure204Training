using Azure.Storage.Blobs;

namespace AppWithBlobStorage.Services
{
    public interface IBlobService
    {
        BlobServiceClient GetBlobServiceClient();
        BlobContainerClient GetContainerClient();
        string GetBlobContainerName();
        Task<bool> UploadToBlobStorageAsync(Stream stream, string name);
        Task<bool> DeleteFromBlobStorageAsync(string name);
    }
}
