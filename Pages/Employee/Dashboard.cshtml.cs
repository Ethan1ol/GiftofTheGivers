using GiftOfTheGiversPrototype.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftOfTheGiversPrototype.Pages.Employee;
public class DashboardModel : PageModel
{
    public PrototypeStore Store { get; }
    [BindProperty] public string Title { get; set; } = "";
    [BindProperty] public string Message { get; set; } = "";
    public DashboardModel(PrototypeStore store) => Store = store;
    public IActionResult OnPost()
    {
        if (!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Message))
            Store.Updates.Add(new Models.ReliefUpdate { Id = Store.Updates.Count + 1, Title = Title, Message = Message });
        return RedirectToPage();
    }
}