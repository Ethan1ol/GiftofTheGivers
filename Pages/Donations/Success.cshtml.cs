using GiftOfTheGiversPrototype.Models;
using GiftOfTheGiversPrototype.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace GiftOfTheGiversPrototype.Pages.Donations;

public class TaxCertificateResponse
{
    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    [JsonPropertyName("CertificateNumber")]
    public string? CertificateNumber { get; set; }

    [JsonPropertyName("DonationAmount")]
    public decimal DonationAmount { get; set; }

    [JsonPropertyName("DonorName")]
    public string? DonorName { get; set; }

    [JsonPropertyName("Organization")]
    public string? Organization { get; set; }
}

public class SuccessModel : PageModel
{
    private readonly PrototypeStore _store;
    private readonly IHttpClientFactory _httpClientFactory;

    public SuccessModel(PrototypeStore store, IHttpClientFactory httpClientFactory)
    {
        _store = store;
        _httpClientFactory = httpClientFactory;
    }

    public Donation? Donation { get; set; }
    public TaxCertificateResponse? Certificate { get; set; }

    public async Task OnGetAsync(int id)
    {
        Donation = _store.Donations.FirstOrDefault(x => x.Id == id);

        if (Donation != null)
        {
            var client = _httpClientFactory.CreateClient();
            var functionUrl = "https://duodominance-giftofthegivers-fpdgahc9heeaabf3.southafricanorth-01.azurewebsites.net/api/donations/certificate";

            var payload = new
            {
                DonorName = Donation.DonorName,
                Amount = Donation.Amount
            };

            var response = await client.PostAsJsonAsync(functionUrl, payload);
            if (response.IsSuccessStatusCode)
            {
                Certificate = await response.Content.ReadFromJsonAsync<TaxCertificateResponse>();
            }
        }
    }
}