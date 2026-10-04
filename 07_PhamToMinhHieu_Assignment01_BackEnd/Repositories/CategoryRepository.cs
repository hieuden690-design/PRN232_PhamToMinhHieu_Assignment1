using System.Collections.Generic;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DataAccess;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

public class CategoryRepository : ICategoryRepository
{
    public List<Category> GetCategories() => CategoryDAO.Instance.GetCategories();

    public Category? GetCategoryById(short id) => CategoryDAO.Instance.GetCategoryById(id);

    public void AddCategory(Category category) => CategoryDAO.Instance.AddCategory(category);

    public void UpdateCategory(Category category) => CategoryDAO.Instance.UpdateCategory(category);

    public bool DeleteCategory(short id, out string errorMessage) => CategoryDAO.Instance.DeleteCategory(id, out errorMessage);

    public List<Category> SearchCategories(string keyword) => CategoryDAO.Instance.SearchCategories(keyword);
}
