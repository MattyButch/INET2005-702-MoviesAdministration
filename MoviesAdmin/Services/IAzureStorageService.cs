namespace MoviesAdmin.Services
{
    public interface IAzureStorageService
    {
        //Service to upload a file to Azure Blob Storage
        //Return: the URL of the uploaded file as a string 
        public Task<string> UploadFileAsync(IFormFile file, string fileName);


        //To do: add a new function to delete a file to handle when a trail record is deleted
    }
}
