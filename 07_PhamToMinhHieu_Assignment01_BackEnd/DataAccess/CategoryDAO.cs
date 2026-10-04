using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.DataAccess;

public class CategoryDAO
{
    private static CategoryDAO? _instance;
    private static readonly object _lock = new object();

    private CategoryDAO() { }

    public static CategoryDAO Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new CategoryDAO();
                }
                return _instance;
            }
        }
    }

    public List<Category> GetCategories()
    {
        using var context = new FUNewsManagementDbContext();
        return context.Categories
            .Include(c => c.ParentCategory)
            .ToList();
    }

    public Category? GetCategoryById(short id)
    {
        using var context = new FUNewsManagementDbContext();
        return context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefault(c => c.CategoryId == id);
    }

    public void AddCategory(Category category)
    {
        using var context = new FUNewsManagementDbContext();
        context.Categories.Add(category);
        context.SaveChanges();
    }

    public void UpdateCategory(Category category)
    {
        using var context = new FUNewsManagementDbContext();
        var existing = context.Categories.Find(category.CategoryId);
        if (existing != null)
        {
            existing.CategoryName = category.CategoryName;
            existing.CategoryDesciption = category.CategoryDesciption;
            existing.ParentCategoryId = category.ParentCategoryId;
            existing.IsActive = category.IsActive;
            context.SaveChanges();
        }
    }

    public bool DeleteCategory(short id, out string errorMessage)
    {
        using var context = new FUNewsManagementDbContext();
        // Check if category has any news articles
        bool hasArticles = context.NewsArticles.Any(n => n.CategoryId == id);
        if (hasArticles)
        {
            errorMessage = "Cannot delete this category because it is already used in one or more news articles.";
            return false;
        }

        // Also check if it is a parent category of other categories
        bool hasChildCategories = context.Categories.Any(c => c.ParentCategoryId == id);
        if (hasChildCategories)
        {
            errorMessage = "Cannot delete this category because other categories depend on it as a parent category.";
            return false;
        }

        var category = context.Categories.Find(id);
        if (category == null)
        {
            errorMessage = "Category not found.";
            return false;
        }

        context.Categories.Remove(category);
        context.SaveChanges();
        errorMessage = string.Empty;
        return true;
    }

    public List<Category> SearchCategories(string keyword)
    {
        using var context = new FUNewsManagementDbContext();
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return GetCategories();
        }

        keyword = keyword.Trim().ToLower();
        return context.Categories
            .Include(c => c.ParentCategory)
            .Where(c => c.CategoryName.ToLower().Contains(keyword) ||
                        c.CategoryDesciption.ToLower().Contains(keyword))
            .ToList();
    }
}
