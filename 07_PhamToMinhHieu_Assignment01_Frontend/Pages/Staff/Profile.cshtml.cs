using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _07_PhamToMinhHieu_Assignment01_Frontend.Models;
using _07_PhamToMinhHieu_Assignment01_Frontend.Services;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Pages.Staff;

public class ProfileModel : PageModel
{
    private readonly ApiClient _apiClient;

    public ProfileModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty]
    public AccountViewModel ProfileForm { get; set; } = new AccountViewModel();

    [TempData]
    public string? SuccessMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff")
        {
            return RedirectToPage("/Login");
        }

        short.TryParse(HttpContext.Session.GetString("AccountId"), out short currentUserId);

        var account = await _apiClient.GetAsync<AccountViewModel>($"Accounts/{currentUserId}");
        if (account == null)
        {
            return RedirectToPage("/Login");
        }

        ProfileForm = account;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff")
        {
            return RedirectToPage("/Login");
        }

        short.TryParse(HttpContext.Session.GetString("AccountId"), out short currentUserId);

        var (success, error) = await _apiClient.PutAsync($"Accounts/{currentUserId}", new
        {
            AccountId = currentUserId,
            AccountName = ProfileForm.AccountName,
            AccountEmail = ProfileForm.AccountEmail,
            AccountRole = 1, // Staff remains Staff
            AccountPassword = ProfileForm.AccountPassword
        });

        if (success)
        {
            SuccessMessage = "Profile updated successfully!";
            // Update session
            HttpContext.Session.SetString("AccountName", ProfileForm.AccountName);
            HttpContext.Session.SetString("AccountEmail", ProfileForm.AccountEmail);
        }
        else
        {
            ErrorMessage = error ?? "Failed to update profile.";
        }

        return RedirectToPage();
    }
}
