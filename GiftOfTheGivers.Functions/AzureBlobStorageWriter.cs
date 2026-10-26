using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace GiftOfTheGivers.Function;

public static class AzureBlobStorageWriter
{
    public static async Task SaveJsonAsync(
        string? connectionString,
        string containerName,
        string folder,
        object record,
        string filePrefix,
        ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("Storage connection string is not configured. Record was not saved.");
            return;
        }

        try
        {
            var blobEmployee = new BlobServiceClient(connectionString);
            var containerClient = blobEmployee.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobEmployeeName = $"{folder}/{DateTime.UtcNow:yyyyMMdd}/{filePrefix}-{Guid.NewGuid():N}.json";
            var blobClient = containerClient.GetBlobClient(blobEmployeeName);
            var json = JsonSerializer.Serialize(record);

            await blobClient.UploadAsync(
                BinaryData.FromBytes(Encoding.UTF8.GetBytes(json)),
                overwrite: true);

            logger.LogInformation("Saved record to Azure Blob Storage: {BlobName}", blobEmployeeName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save record to Azure Blob Storage in container {ContainerName}", containerName);
            throw;
        }
    }
}
