using Azure.Storage;
using Azure.Storage.Blobs;

namespace CarShop.Cloud;

public class FileService
{
    private readonly string _storageAccount = "movsdav";
    private readonly string _key = "a4TK2a23l+tKFpmnS52/Ang+XUwLiAm4Qoehay6Y92FudKicyud6WQdRDvncKXERlNwMWGfhXH0o+AStx1QDyg==";
    private readonly BlobContainerClient _filesContainer;

    public FileService()
    {
        var credential = new StorageSharedKeyCredential(_storageAccount, _key);
        var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
        var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        _filesContainer = blobServiceClient.GetBlobContainerClient("carshopapp-2023");
    }

    public async Task<BlobDto?> GetBlobAsync(string? name)
    {
        if (name == null) return null;

        var images = await ListAsync();

        foreach(var img in images)
        {
            if(img.Name == name)
            {
                return img;
            }
        }

        return null;
    }

    public async Task<List<BlobDto>> ListAsync()
    {
        List<BlobDto> files = new List<BlobDto>();

        await foreach (var file in _filesContainer.GetBlobsAsync())
        {
            string uri = _filesContainer.Uri.ToString();
            var name = file.Name;
            var fullUri = $"{uri}/{name}";

            files.Add(new BlobDto
            {
                Uri = fullUri,
                Name = name,
                ContentType = file.Properties.ContentType
            });
        }

        return files;
    }

    public async Task<BlobResponseDto> UploadAsync(IFormFile blob,string prefix="")
    {
        BlobResponseDto response = new BlobResponseDto();
        BlobClient client = _filesContainer.GetBlobClient(prefix+blob.FileName);

        await using (Stream? data = blob.OpenReadStream())
        {
            await client.UploadAsync(data);
        }

        response.Status = $"File {blob.FileName} Uploaded Successfully";
        response.Error = false;
        response.Blob.Uri = client.Uri.AbsoluteUri;
        response.Blob.Name = client.Name;

        return response;
    }


    public async Task<BlobDto?> DownloadAsync(string blobFilename)
    {
        BlobClient file = _filesContainer.GetBlobClient(blobFilename);

        if (await file.ExistsAsync())
        {
            var data = await file.OpenReadAsync();
            Stream blobContent = data;

            var content = await file.DownloadContentAsync();

            string name = blobFilename;
            string contentType = content.Value.Details.ContentType;

            return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
        }

        return null;
    }

    public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
    {
        BlobClient file = _filesContainer.GetBlobClient(blobFilename);

        await file.DeleteAsync();

        return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been deleted." };
    }

    public BlobResponseDto Delete(string blobFilename)
    {
        BlobClient file = _filesContainer.GetBlobClient(blobFilename);

        file.Delete();

        return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been deleted." };
    }
}
