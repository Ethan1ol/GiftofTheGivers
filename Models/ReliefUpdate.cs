namespace GiftOfTheGiversPrototype.Models;

public class ReliefUpdate
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}