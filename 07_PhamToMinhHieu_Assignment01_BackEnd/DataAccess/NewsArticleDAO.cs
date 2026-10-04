using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.DataAccess;

public class NewsArticleDAO
{
    private static NewsArticleDAO? _instance;
    private static readonly object _lock = new object();

    private NewsArticleDAO() { }

    public static NewsArticleDAO Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new NewsArticleDAO();
                }
                return _instance;
            }
        }
    }

    public List<NewsArticle> GetAllArticles()
    {
        using var context = new FUNewsManagementDbContext();
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();
    }

    public IQueryable<NewsArticle> GetArticlesQueryable(FUNewsManagementDbContext context)
    {
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags);
    }

    public List<NewsArticle> GetActiveArticles()
    {
        using var context = new FUNewsManagementDbContext();
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .Where(n => n.NewsStatus == true)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();
    }

    public NewsArticle? GetArticleById(string id)
    {
        using var context = new FUNewsManagementDbContext();
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .FirstOrDefault(n => n.NewsArticleId == id);
    }

    public List<NewsArticle> GetArticlesByCreatedBy(short createdById)
    {
        using var context = new FUNewsManagementDbContext();
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .Where(n => n.CreatedById == createdById)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();
    }

    public List<NewsArticle> GetReport(DateTime startDate, DateTime endDate)
    {
        using var context = new FUNewsManagementDbContext();
        // Inclusive of end date (end of day)
        var endOfDay = endDate.Date.AddDays(1).AddTicks(-1);
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endOfDay)
            .OrderByDescending(n => n.CreatedDate)
            .ToList();
    }

    public void AddArticle(NewsArticle article, List<int> tagIds)
    {
        using var context = new FUNewsManagementDbContext();
        article.CreatedDate = article.CreatedDate ?? DateTime.Now;
        article.ModifiedDate = DateTime.Now;

        if (tagIds != null && tagIds.Any())
        {
            var tags = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
            foreach (var tag in tags)
            {
                article.Tags.Add(tag);
            }
        }

        context.NewsArticles.Add(article);
        context.SaveChanges();
    }

    public void UpdateArticle(NewsArticle article, List<int>? tagIds)
    {
        using var context = new FUNewsManagementDbContext();
        var existing = context.NewsArticles
            .Include(n => n.Tags)
            .FirstOrDefault(n => n.NewsArticleId == article.NewsArticleId);

        if (existing != null)
        {
            existing.NewsTitle = article.NewsTitle;
            existing.Headline = article.Headline;
            existing.NewsContent = article.NewsContent;
            existing.NewsSource = article.NewsSource;
            existing.CategoryId = article.CategoryId;
            existing.NewsStatus = article.NewsStatus;
            existing.UpdatedById = article.UpdatedById;
            existing.ModifiedDate = DateTime.Now;

            if (tagIds != null)
            {
                existing.Tags.Clear();
                var tags = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
                foreach (var tag in tags)
                {
                    existing.Tags.Add(tag);
                }
            }

            context.SaveChanges();
        }
    }

    public bool DeleteArticle(string id, out string errorMessage)
    {
        using var context = new FUNewsManagementDbContext();
        var article = context.NewsArticles
            .Include(n => n.Tags)
            .FirstOrDefault(n => n.NewsArticleId == id);

        if (article == null)
        {
            errorMessage = "News article not found.";
            return false;
        }

        article.Tags.Clear();
        context.NewsArticles.Remove(article);
        context.SaveChanges();
        errorMessage = string.Empty;
        return true;
    }

    public List<NewsArticle> SearchArticles(string keyword)
    {
        using var context = new FUNewsManagementDbContext();
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return GetAllArticles();
        }

        keyword = keyword.Trim().ToLower();
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .Where(n => (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword)) ||
                        (n.Headline != null && n.Headline.ToLower().Contains(keyword)) ||
                        (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword)))
            .OrderByDescending(n => n.CreatedDate)
            .ToList();
    }
}
