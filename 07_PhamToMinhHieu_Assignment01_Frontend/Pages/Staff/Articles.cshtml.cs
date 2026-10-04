using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _07_PhamToMinhHieu_Assignment01_Frontend.Models;
using _07_PhamToMinhHieu_Assignment01_Frontend.Services;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Pages.Staff;

public class ArticlesModel : PageModel
{
    private readonly ApiClient _apiClient;

    public ArticlesModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<NewsArticleViewModel> Articles { get; set; } = new List<NewsArticleViewModel>();
    public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    public List<TagViewModel> AvailableTags { get; set; } = new List<TagViewModel>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty]
    public NewsArticleViewModel ArticleForm { get; set; } = new NewsArticleViewModel();

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

        await LoadLookupsAsync();

        string endpoint = "NewsArticles";
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            endpoint += $"?search={System.Uri.EscapeDataString(SearchTerm)}";
        }

        var result = await _apiClient.GetAsync<List<NewsArticleViewModel>>(endpoint);
        Articles = result ?? new List<NewsArticleViewModel>();
        return Page();
    }

    private async Task LoadLookupsAsync()
    {
        var cats = await _apiClient.GetAsync<List<CategoryViewModel>>("Categories");
        Categories = cats?.Where(c => c.IsActive == true).ToList() ?? new List<CategoryViewModel>();

        var tags = await _apiClient.GetAsync<List<TagViewModel>>("Tags");
        AvailableTags = tags ?? new List<TagViewModel>();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff") return RedirectToPage("/Login");

        short.TryParse(HttpContext.Session.GetString("AccountId"), out short currentUserId);

        var (success, _, error) = await _apiClient.PostAsync<NewsArticleViewModel>("NewsArticles", new
        {
            NewsArticleId = ArticleForm.NewsArticleId,
            NewsTitle = ArticleForm.NewsTitle,
            Headline = ArticleForm.Headline,
            NewsContent = ArticleForm.NewsContent,
            NewsSource = ArticleForm.NewsSource,
            CategoryId = ArticleForm.CategoryId,
            NewsStatus = ArticleForm.NewsStatus,
            CreatedById = currentUserId,
            TagIds = ArticleForm.SelectedTagIds
        });

        if (success)
        {
            SuccessMessage = "News article created successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Failed to create news article.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff") return RedirectToPage("/Login");

        short.TryParse(HttpContext.Session.GetString("AccountId"), out short currentUserId);

        var (success, error) = await _apiClient.PutAsync($"NewsArticles/{ArticleForm.NewsArticleId}", new
        {
            NewsArticleId = ArticleForm.NewsArticleId,
            NewsTitle = ArticleForm.NewsTitle,
            Headline = ArticleForm.Headline,
            NewsContent = ArticleForm.NewsContent,
            NewsSource = ArticleForm.NewsSource,
            CategoryId = ArticleForm.CategoryId,
            NewsStatus = ArticleForm.NewsStatus,
            UpdatedById = currentUserId,
            TagIds = ArticleForm.SelectedTagIds
        });

        if (success)
        {
            SuccessMessage = "News article updated successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Failed to update news article.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff") return RedirectToPage("/Login");

        var (success, error) = await _apiClient.DeleteAsync($"NewsArticles/{id}");

        if (success)
        {
            SuccessMessage = "News article deleted successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Failed to delete news article.";
        }

        return RedirectToPage();
    }
}
