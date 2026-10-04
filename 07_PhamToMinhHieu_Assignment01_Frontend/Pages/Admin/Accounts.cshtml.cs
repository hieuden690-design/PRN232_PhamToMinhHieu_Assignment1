using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _07_PhamToMinhHieu_Assignment01_Frontend.Models;
using _07_PhamToMinhHieu_Assignment01_Frontend.Services;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Pages.Admin;

public class AccountsModel : PageModel
{
    private readonly ApiClient _apiClient;

    public AccountsModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<AccountViewModel> Accounts { get; set; } = new List<AccountViewModel>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty]
    public AccountViewModel AccountForm { get; set; } = new AccountViewModel();

    [TempData]
    public string? SuccessMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            return RedirectToPage("/Login");
        }

        string endpoint = "Accounts";
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            endpoint += $"?search={System.Uri.EscapeDataString(SearchTerm)}";
        }

        var result = await _apiClient.GetAsync<List<AccountViewModel>>(endpoint);
        Accounts = result ?? new List<AccountViewModel>();
        return Page();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin") return RedirectToPage("/Login");

        if (string.IsNullOrWhiteSpace(AccountForm.AccountPassword))
        {
            ErrorMessage = "Password is required for new accounts.";
            return RedirectToPage();
        }

        var (success, _, error) = await _apiClient.PostAsync<AccountViewModel>("Accounts", new
        {
            AccountName = AccountForm.AccountName,
            AccountEmail = AccountForm.AccountEmail,
            AccountRole = AccountForm.AccountRole,
            AccountPassword = AccountForm.AccountPassword
        });

        if (success)
        {
            SuccessMessage = "Account created successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Failed to create account.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin") return RedirectToPage("/Login");

        var (success, error) = await _apiClient.PutAsync($"Accounts/{AccountForm.AccountId}", new
        {
            AccountId = AccountForm.AccountId,
            AccountName = AccountForm.AccountName,
            AccountEmail = AccountForm.AccountEmail,
            AccountRole = AccountForm.AccountRole,
            AccountPassword = AccountForm.AccountPassword
        });

        if (success)
        {
            SuccessMessage = "Account updated successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Failed to update account.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(short id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin") return RedirectToPage("/Login");

        var (success, error) = await _apiClient.DeleteAsync($"Accounts/{id}");

        if (success)
        {
            SuccessMessage = "Account deleted successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Cannot delete account.";
        }

        return RedirectToPage();
    }
}
