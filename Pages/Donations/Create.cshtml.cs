using GiftOfTheGiversPrototype.Models;
using GiftOfTheGiversPrototype.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGiversPrototype.Pages.Donations;
public class CreateModel : PageModel
{
    private readonly PrototypeStore _store;
    public CreateModel(PrototypeStore store) => _store = store;

    [BindProperty] public InputModel Input { get; set; } = new();
    public int CertificateId { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        var donation = new Donation {
            Id = _store.Donations.Count + 1,
            DonorName = string.IsNullOrWhiteSpace(Input.Name) ? "Anonymous" : Input.Name,
            Amount = Input.Amount, Currency = Input.Currency, Type = Input.Type
        };
        _store.Donations.Add(donation);
        return RedirectToPage("/Donations/Success", new { id = donation.Id });
    }

    public class InputModel
    {
        public string? Name { get; set; }
        [Range(1, 1000000)] public decimal Amount { get; set; }
        public string Currency { get; set; } = "ZAR";
        public string Type { get; set; } = "One-time";
    }
}