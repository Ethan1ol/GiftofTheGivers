using GiftOfTheGiversPrototype.Models;

namespace GiftOfTheGiversPrototype.Services;

public class PrototypeStore
{
    public List<Donation> Donations { get; } = new();
    public List<Volunteer> Volunteers { get; } = new();
    public List<ReliefUpdate> Updates { get; } = new()
    {
        new ReliefUpdate { Id = 1, Title = "Winter Relief Drive", Message = "Our teams are preparing essential supplies for communities affected by winter conditions." },
        new ReliefUpdate { Id = 2, Title = "Food Distribution", Message = "Food parcels are being distributed through local relief partners." }
    };
}