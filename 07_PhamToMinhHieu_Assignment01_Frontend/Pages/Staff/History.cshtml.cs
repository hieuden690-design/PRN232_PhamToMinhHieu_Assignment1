using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _07_PhamToMinhHieu_Assignment01_Frontend.Models;
using _07_PhamToMinhHieu_Assignment01_Frontend.Services;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Pages.Staff;

public class HistoryModel : PageModel
{
    private readonly ApiClient _apiClient;

    public HistoryModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<NewsArticleViewModel> HistoryArticles { get; set; } = new List<NewsArticleViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff")
        {
            return RedirectToPage("/Login");
        }

        short.TryParse(HttpContext.Session.GetString("AccountId"), out short currentUserId);

        var result = await _apiClient.GetAsync<List<NewsArticleViewModel>>($"NewsArticles/history/{currentUserId}");
        HistoryArticles = result ?? new List<NewsArticleViewModel>();

        return Page();
    }
}
