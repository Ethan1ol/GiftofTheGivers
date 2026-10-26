using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GiftOfTheGivers.Function;

public class EmployeeUpdateRequest
{
    public string? EmployeeName { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
}

public class EmployeeUpdateLogger
{
    private readonly ILogger _logger;

    public EmployeeUpdateLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<EmployeeUpdateLogger>();
    }

    [Function("EmployeeUpdate")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employee/updates")] HttpRequestData req)
    {
        _logger.LogInformation("EmployeeUpdate function processed a request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        EmployeeUpdateRequest? updateRequest = null;

        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                updateRequest = JsonSerializer.Deserialize<EmployeeUpdateRequest>(
                    body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badResponse.Headers.Add("Content-Type", "application/json");
                await badResponse.WriteStringAsync("Invalid employee update");
                return badResponse;
            }
        }

        if (updateRequest == null ||
            string.IsNullOrWhiteSpace(updateRequest.EmployeeName) ||
            string.IsNullOrWhiteSpace(updateRequest.Message))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Content-Type", "application/json");
            await errorResponse.WriteStringAsync("EmployeeName and Message are required.");
            return errorResponse;
        }

        var storageConnection = Environment.GetEnvironmentVariable("DonationStorageConnection")
            ?? Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        var record = new
        {
            EmployeeName = updateRequest.EmployeeName,
            Title = string.IsNullOrWhiteSpace(updateRequest.Title) ? "General Update" : updateRequest.Title,
            Message = updateRequest.Message,
            PostedUtc = DateTime.UtcNow
        };

        try
        {
            await AzureBlobStorageWriter.SaveJsonAsync(
                storageConnection,
                "employee-updates",
                "updates",
                record,
                "employee-update",
                _logger);
        }
        catch (Exception)
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            errorResponse.Headers.Add("Content-Type", "application/json");
            await errorResponse.WriteStringAsync("Failed to log employee update to Azure Storage.");
            return errorResponse;
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(JsonSerializer.Serialize(new
        {
            Status = "Employee update logged",
            EmployeeName = updateRequest.EmployeeName,
            Title = string.IsNullOrWhiteSpace(updateRequest.Title) ? "General Update" : updateRequest.Title,
            PostedUtc = DateTime.UtcNow
        }));

        return response;
    }
}
