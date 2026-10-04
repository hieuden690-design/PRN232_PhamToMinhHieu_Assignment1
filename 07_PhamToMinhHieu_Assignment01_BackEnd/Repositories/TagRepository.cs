using System.Collections.Generic;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DataAccess;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

public class TagRepository : ITagRepository
{
    public List<Tag> GetTags() => TagDAO.Instance.GetTags();

    public Tag? GetTagById(int id) => TagDAO.Instance.GetTagById(id);
}
