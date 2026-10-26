namespace GiftOfTheGiversPrototype.Models;

public class Volunteer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Skills { get; set; } = "";
    public string Availability { get; set; } = "";
    public DateTime SubmittedAt { get; set; } = DateTime.Now;
}