using System.Collections.Generic;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

public interface ITagRepository
{
    List<Tag> GetTags();
    Tag? GetTagById(int id);
}
