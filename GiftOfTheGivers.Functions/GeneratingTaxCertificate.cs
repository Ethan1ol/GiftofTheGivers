using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GiftOfTheGivers.Function
{
    public class RequestTaxCertificate
    {
        public string? DonorName { get; set; }
        public decimal Amount { get; set; }
    }

    public class GeneratingTaxCertificate
    {
        private readonly ILogger _logger;

        public GeneratingTaxCertificate(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GeneratingTaxCertificate>();
        }

        [Function("GeneratingTaxCertificate")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "donations/certificate")] HttpRequestData req)
        {
            _logger.LogInformation("GeneratingTaxCertificate function processed a request.");

            string body = await new StreamReader(req.Body).ReadToEndAsync();
            RequestTaxCertificate? donationInput = null;

            if (!string.IsNullOrWhiteSpace(body))
            {
                try
                {
                    donationInput = JsonSerializer.Deserialize<RequestTaxCertificate>(
                        body,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch (JsonException)
                {
                    var responseBad = req.CreateResponse(HttpStatusCode.BadRequest);
                    responseBad.Headers.Add("Content-Type", "application/json");
                    await responseBad.WriteStringAsync("Invalid Amount. Please enter value greater than 0.");
                    return responseBad;
                }
            }

            if (donationInput == null || donationInput.Amount <= 0)
            {
                var responseError = req.CreateResponse();
                responseError.StatusCode = HttpStatusCode.BadRequest;
                responseError.Headers.Add("Content-Type", "application/json");
                await responseError.WriteStringAsync("Invalid Amount. Please enter value greater than 0.");
                return responseError;
            }    
            
            var result = new
            {
                Status = "Successful Donation",

                CertificateNumber = $"GOTD-TAX-{DateTime.UtcNow:yyyyMMddHHmmss}",
               
                DonationAmount = donationInput.Amount,
                DonorName = string.IsNullOrWhiteSpace(donationInput.DonorName) ? "Anonymous" : donationInput.DonorName,
                Organization = "Gift of the Givers Foundation"
            };


            var storageConnection = Environment.GetEnvironmentVariable("DonationStorageConnection")
            ?? Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            if (!string.IsNullOrWhiteSpace(storageConnection)){
            try
            { var blobDonor = new BlobServiceClient(storageConnection);
            var containerClient = blobDonor.GetBlobContainerClient("donation-records");
            await containerClient.CreateIfNotExistsAsync();
            
            var record = new
            {
                DonorName = result.DonorName,
                DonationAmount = result.DonationAmount,
                CertificateNumber = result.CertificateNumber,
                Organization = result.Organization,
                CreatedUtc = DateTime.UtcNow
            };
            var blobName = $"donation-{DateTime.UtcNow:yyyyMMddHHmmss}.json";
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(
                BinaryData.FromBytes(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(record))),
                overwrite: true);
            _logger.LogInformation("Donation certificate saved to Azure Blob Storage: {BlobName}", blobName);
            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save donation certificate to Storage");
            }
            }
        else
        {
            _logger.LogWarning("DonationStorageConnection or AzureWebJobsStorage environment variable is not set. Skipping saving to storage.");
        }
            

            var responseSuccess = req.CreateResponse(HttpStatusCode.OK);
            responseSuccess.Headers.Add("Content-Type", "application/json");
            await responseSuccess.WriteStringAsync(JsonSerializer.Serialize(result));
            return responseSuccess;
        }
    }
}
