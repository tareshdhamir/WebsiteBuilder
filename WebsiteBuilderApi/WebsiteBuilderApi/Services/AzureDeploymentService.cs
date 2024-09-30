using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace WebsiteBuilderApi.Services
{
    public class AzureDeploymentService
    {
        private readonly string _storageConnectionString;

        public AzureDeploymentService(IConfiguration configuration)
        {
            _storageConnectionString = configuration["Azure:StorageConnectionString"];
        }

        public async Task<string> DeployTemplateAsync(string templatePath, string containerName)
        {
            // Get the full path of the template
            var fullTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);

            BlobServiceClient blobServiceClient = new BlobServiceClient(_storageConnectionString);
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            // Set the container's access level to Blob (public)
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            // Upload the template files
            var files = Directory.GetFiles(Path.GetDirectoryName(fullTemplatePath));

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(file, overwrite: true);
            }

            // Return the URL of the deployed site
            return $"{containerClient.Uri}/index.html";
        }
    }
}
