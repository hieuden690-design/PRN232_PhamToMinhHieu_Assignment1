using System;
using System.Collections.Generic;
using System.Linq;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DataAccess;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

public class NewsArticleRepository : INewsArticleRepository
{
    public List<NewsArticle> GetAllArticles() => NewsArticleDAO.Instance.GetAllArticles();

    public IQueryable<NewsArticle> GetArticlesQueryable(FUNewsManagementDbContext context) => NewsArticleDAO.Instance.GetArticlesQueryable(context);

    public List<NewsArticle> GetActiveArticles() => NewsArticleDAO.Instance.GetActiveArticles();

    public NewsArticle? GetArticleById(string id) => NewsArticleDAO.Instance.GetArticleById(id);

    public List<NewsArticle> GetArticlesByCreatedBy(short createdById) => NewsArticleDAO.Instance.GetArticlesByCreatedBy(createdById);

    public List<NewsArticle> GetReport(DateTime startDate, DateTime endDate) => NewsArticleDAO.Instance.GetReport(startDate, endDate);

    public void AddArticle(NewsArticle article, List<int> tagIds) => NewsArticleDAO.Instance.AddArticle(article, tagIds);

    public void UpdateArticle(NewsArticle article, List<int>? tagIds) => NewsArticleDAO.Instance.UpdateArticle(article, tagIds);

    public bool DeleteArticle(string id, out string errorMessage) => NewsArticleDAO.Instance.DeleteArticle(id, out errorMessage);

    public List<NewsArticle> SearchArticles(string keyword) => NewsArticleDAO.Instance.SearchArticles(keyword);
}
