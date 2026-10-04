using System;
using System.Collections.Generic;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;

public partial class Tag
{
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
