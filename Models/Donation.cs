namespace GiftOfTheGiversPrototype.Models;

public class Donation
{
    public int Id { get; set; }
    public string DonorName { get; set; } = "Anonymous";
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "ZAR";
    public string Type { get; set; } = "One-time";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}