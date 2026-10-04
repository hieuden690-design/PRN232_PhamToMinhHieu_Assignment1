using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _07_PhamToMinhHieu_Assignment01_Frontend.Models;
using _07_PhamToMinhHieu_Assignment01_Frontend.Services;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Pages;

public class LoginModel : PageModel
{
    private readonly ApiClient _apiClient;

    public LoginModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public LoginViewModel Input { get; set; } = new LoginViewModel();

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var (success, user, error) = await _apiClient.PostAsync<UserSessionDto>("auth/login", new
        {
            Email = Input.Email,
            Password = Input.Password
        });

        if (!success || user == null)
        {
            ErrorMessage = error ?? "Invalid email or password.";
            return Page();
        }

        // Store session
        HttpContext.Session.SetString("AccountId", user.AccountId.ToString());
        HttpContext.Session.SetString("AccountName", user.AccountName);
        HttpContext.Session.SetString("AccountEmail", user.AccountEmail);
        HttpContext.Session.SetString("Role", user.Role);

        if (user.Role == "Admin")
        {
            return RedirectToPage("/Admin/Accounts");
        }
        else if (user.Role == "Staff")
        {
            return RedirectToPage("/Staff/Articles");
        }

        return RedirectToPage("/Index");
    }
}
