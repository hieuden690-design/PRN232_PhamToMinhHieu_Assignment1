using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _07_PhamToMinhHieu_Assignment01_Frontend.Models;
using _07_PhamToMinhHieu_Assignment01_Frontend.Services;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Pages.Staff;

public class CategoriesModel : PageModel
{
    private readonly ApiClient _apiClient;

    public CategoriesModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty]
    public CategoryViewModel CategoryForm { get; set; } = new CategoryViewModel();

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

        string endpoint = "Categories";
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            endpoint += $"?search={System.Uri.EscapeDataString(SearchTerm)}";
        }

        var result = await _apiClient.GetAsync<List<CategoryViewModel>>(endpoint);
        Categories = result ?? new List<CategoryViewModel>();
        return Page();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff") return RedirectToPage("/Login");

        var (success, _, error) = await _apiClient.PostAsync<CategoryViewModel>("Categories", new
        {
            CategoryName = CategoryForm.CategoryName,
            CategoryDesciption = CategoryForm.CategoryDesciption,
            ParentCategoryId = CategoryForm.ParentCategoryId,
            IsActive = CategoryForm.IsActive
        });

        if (success)
        {
            SuccessMessage = "Category created successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Failed to create category.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff") return RedirectToPage("/Login");

        var (success, error) = await _apiClient.PutAsync($"Categories/{CategoryForm.CategoryId}", new
        {
            CategoryId = CategoryForm.CategoryId,
            CategoryName = CategoryForm.CategoryName,
            CategoryDesciption = CategoryForm.CategoryDesciption,
            ParentCategoryId = CategoryForm.ParentCategoryId,
            IsActive = CategoryForm.IsActive
        });

        if (success)
        {
            SuccessMessage = "Category updated successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Failed to update category.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(short id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Staff") return RedirectToPage("/Login");

        var (success, error) = await _apiClient.DeleteAsync($"Categories/{id}");

        if (success)
        {
            SuccessMessage = "Category deleted successfully!";
        }
        else
        {
            ErrorMessage = error ?? "Cannot delete category.";
        }

        return RedirectToPage();
    }
}
