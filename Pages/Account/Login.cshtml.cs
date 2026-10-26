using GiftOfTheGiversPrototype.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGiversPrototype.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signIn;
    public LoginModel(SignInManager<ApplicationUser> signIn) => _signIn = signIn;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required, EmailAddress] public string Email { get; set; } = "";
        [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var result = await _signIn.PasswordSignInAsync(Input.Email, Input.Password, true, false);
        if (result.Succeeded)
        {
            if (Input.Email.Equals("employee@givers.org", StringComparison.OrdinalIgnoreCase))
                return RedirectToPage("/Employee/Dashboard");
            return RedirectToPage("/Index");
        }
        ModelState.AddModelError("", "Invalid login details.");
        return Page();
    }
}