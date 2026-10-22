using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MoviesAdmin.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly IConfiguration _configuration;

        private readonly BlobServiceClient blobServiceClient;
        private string containerName;

    //Constructor
    //Allows you to extract the connection string 
        public AzureStorageService(IConfiguration configuration)
        {
            //Configuration is used to get appsettings, user-secrets, env vars, etc
            _configuration = configuration;
            //Gets Azure Storage Container name from appsettings.json
            containerName = _configuration["AzureStorage:ContainerName"];


            //gets Azure storage connection string
            string connectionString = _configuration["AzureStorage:ConnectionString"];


            //Initialize Azure blob client
            blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string filename)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(filename);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            };


            // file I/O
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, options);
            }
            return blobClient.Uri.ToString();



        }

    }
}
