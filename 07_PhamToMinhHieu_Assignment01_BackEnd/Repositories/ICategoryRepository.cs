using System.Collections.Generic;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

public interface ICategoryRepository
{
    List<Category> GetCategories();
    Category? GetCategoryById(short id);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    bool DeleteCategory(short id, out string errorMessage);
    List<Category> SearchCategories(string keyword);
}
