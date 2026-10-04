using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _07_PhamToMinhHieu_Assignment01_Frontend.Models;
using _07_PhamToMinhHieu_Assignment01_Frontend.Services;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ApiClient _apiClient;

    public IndexModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<NewsArticleViewModel> Articles { get; set; } = new List<NewsArticleViewModel>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync()
    {
        string endpoint = "NewsArticles/active";
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            endpoint += $"?search={System.Uri.EscapeDataString(SearchTerm)}";
        }

        var result = await _apiClient.GetAsync<List<NewsArticleViewModel>>(endpoint);
        Articles = result ?? new List<NewsArticleViewModel>();
    }
}
