using System.Collections.Generic;
using System.Linq;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.DataAccess;

public class TagDAO
{
    private static TagDAO? _instance;
    private static readonly object _lock = new object();

    private TagDAO() { }

    public static TagDAO Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new TagDAO();
                }
                return _instance;
            }
        }
    }

    public List<Tag> GetTags()
    {
        using var context = new FUNewsManagementDbContext();
        return context.Tags.ToList();
    }

    public Tag? GetTagById(int id)
    {
        using var context = new FUNewsManagementDbContext();
        return context.Tags.Find(id);
    }
}
