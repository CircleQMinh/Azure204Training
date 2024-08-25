using Azure.Storage.Blobs;
using System.IO;

namespace AppWithBlobStorage.Services
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _configuration;
        public BlobService(IConfiguration configuration) { 
        
            _configuration = configuration;
        }

        public string GetBlobContainerName()
        {
            var blobContainerName = _configuration.GetSection("BlobImageContainerName").Value;
            return blobContainerName ?? "";
        }

        public BlobServiceClient GetBlobServiceClient()
        {
            var connectionString = _configuration.GetSection("BlobStorageConnectionString").Value;
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            return blobServiceClient;
        }

        public BlobContainerClient GetContainerClient()
        {
            var client = GetBlobServiceClient();
            BlobContainerClient blobContainer = client.GetBlobContainerClient(GetBlobContainerName());
            return blobContainer;
        }

        public async Task<bool> UploadToBlobStorageAsync(Stream stream, string name)
        {
            try
            {
                var client = GetContainerClient();
                await client.UploadBlobAsync(name, stream);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<bool> DeleteFromBlobStorageAsync(string name)
        {
            try
            {
                var client = GetContainerClient();
                await client.DeleteBlobIfExistsAsync(name);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
