using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;
using _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly INewsArticleRepository _newsArticleRepository;

    public ReportsController(INewsArticleRepository newsArticleRepository)
    {
        _newsArticleRepository = newsArticleRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<NewsArticleResponseDto>> GetReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        DateTime start = startDate ?? DateTime.MinValue;
        DateTime end = endDate ?? DateTime.MaxValue;

        if (start > end)
        {
            return BadRequest(new { message = "StartDate cannot be later than EndDate." });
        }

        var articles = _newsArticleRepository.GetReport(start, end);

        var response = articles.Select(n => new NewsArticleResponseDto
        {
            NewsArticleId = n.NewsArticleId,
            NewsTitle = n.NewsTitle,
            Headline = n.Headline,
            CreatedDate = n.CreatedDate,
            NewsContent = n.NewsContent,
            NewsSource = n.NewsSource,
            CategoryId = n.CategoryId,
            CategoryName = n.Category?.CategoryName,
            NewsStatus = n.NewsStatus,
            CreatedById = n.CreatedById,
            CreatedByName = n.CreatedBy?.AccountName,
            UpdatedById = n.UpdatedById,
            ModifiedDate = n.ModifiedDate,
            Tags = n.Tags?.Select(t => new TagDto
            {
                TagId = t.TagId,
                TagName = t.TagName,
                Note = t.Note
            }).ToList() ?? new List<TagDto>()
        });

        return Ok(response);
    }
}
