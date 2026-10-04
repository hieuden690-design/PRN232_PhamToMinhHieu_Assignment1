using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _07_PhamToMinhHieu_Assignment01_Frontend.Models;
using _07_PhamToMinhHieu_Assignment01_Frontend.Services;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Pages.Admin;

public class ReportModel : PageModel
{
    private readonly ApiClient _apiClient;

    public ReportModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [BindProperty(SupportsGet = true)]
    public DateTime? StartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? EndDate { get; set; }

    public List<NewsArticleViewModel> ReportArticles { get; set; } = new List<NewsArticleViewModel>();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            return RedirectToPage("/Login");
        }

        // Default range: 1 year ago until today if not specified
        if (!StartDate.HasValue && !EndDate.HasValue)
        {
            StartDate = DateTime.Today.AddMonths(-6);
            EndDate = DateTime.Today;
        }

        if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
        {
            ErrorMessage = "Start Date cannot be later than End Date.";
            return Page();
        }

        string sDateStr = StartDate?.ToString("yyyy-MM-dd") ?? "";
        string eDateStr = EndDate?.ToString("yyyy-MM-dd") ?? "";

        var result = await _apiClient.GetAsync<List<NewsArticleViewModel>>($"Reports?startDate={sDateStr}&endDate={eDateStr}");
        ReportArticles = result ?? new List<NewsArticleViewModel>();

        return Page();
    }
}
