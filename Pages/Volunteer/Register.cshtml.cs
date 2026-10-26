using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GiftOfTheGiversPrototype.Services;

namespace GiftOfTheGiversPrototype.Pages.Volunteer
{
    public class RegisterModel : PageModel
    {
        private readonly PrototypeStore _store;

        public RegisterModel(PrototypeStore store)
        {
            _store = store;
        }

        [BindProperty]
        public GiftOfTheGiversPrototype.Models.Volunteer Input { get; set; } = new GiftOfTheGiversPrototype.Models.Volunteer();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _store.Volunteers.Add(Input);

            return RedirectToPage("Success");
        }
    }
}