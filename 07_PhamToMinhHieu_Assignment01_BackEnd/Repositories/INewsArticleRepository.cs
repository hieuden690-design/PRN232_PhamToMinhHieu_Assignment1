using System;
using System.Collections.Generic;
using System.Linq;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

public interface INewsArticleRepository
{
    List<NewsArticle> GetAllArticles();
    IQueryable<NewsArticle> GetArticlesQueryable(FUNewsManagementDbContext context);
    List<NewsArticle> GetActiveArticles();
    NewsArticle? GetArticleById(string id);
    List<NewsArticle> GetArticlesByCreatedBy(short createdById);
    List<NewsArticle> GetReport(DateTime startDate, DateTime endDate);
    void AddArticle(NewsArticle article, List<int> tagIds);
    void UpdateArticle(NewsArticle article, List<int>? tagIds);
    bool DeleteArticle(string id, out string errorMessage);
    List<NewsArticle> SearchArticles(string keyword);
}
