using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;
using _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsArticlesController : ControllerBase
{
    private readonly INewsArticleRepository _newsArticleRepository;

    public NewsArticlesController(INewsArticleRepository newsArticleRepository)
    {
        _newsArticleRepository = newsArticleRepository;
    }

    [HttpGet]
    [EnableQuery]
    public ActionResult<IEnumerable<NewsArticleResponseDto>> GetAll([FromQuery] string? search)
    {
        var articles = string.IsNullOrWhiteSpace(search)
            ? _newsArticleRepository.GetAllArticles()
            : _newsArticleRepository.SearchArticles(search);

        var response = articles.Select(MapToResponseDto);
        return Ok(response);
    }

    [HttpGet("active")]
    [EnableQuery]
    public ActionResult<IEnumerable<NewsArticleResponseDto>> GetActive([FromQuery] string? search)
    {
        var articles = _newsArticleRepository.GetActiveArticles();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim().ToLower();
            articles = articles.Where(n => (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword)) ||
                                           (n.Headline != null && n.Headline.ToLower().Contains(keyword)) ||
                                           (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword))).ToList();
        }

        var response = articles.Select(MapToResponseDto);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public ActionResult<NewsArticleResponseDto> GetById(string id)
    {
        var article = _newsArticleRepository.GetArticleById(id);
        if (article == null)
        {
            return NotFound(new { message = $"News article with ID {id} not found." });
        }

        return Ok(MapToResponseDto(article));
    }

    [HttpGet("history/{createdById}")]
    public ActionResult<IEnumerable<NewsArticleResponseDto>> GetHistory(short createdById)
    {
        var articles = _newsArticleRepository.GetArticlesByCreatedBy(createdById);
        var response = articles.Select(MapToResponseDto);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] NewsArticleCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existing = _newsArticleRepository.GetArticleById(dto.NewsArticleId);
        if (existing != null)
        {
            return BadRequest(new { message = $"Article ID '{dto.NewsArticleId}' already exists." });
        }

        var article = new NewsArticle
        {
            NewsArticleId = dto.NewsArticleId,
            NewsTitle = dto.NewsTitle,
            Headline = dto.Headline,
            NewsContent = dto.NewsContent,
            NewsSource = dto.NewsSource,
            CategoryId = dto.CategoryId,
            NewsStatus = dto.NewsStatus ?? true,
            CreatedById = dto.CreatedById,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        _newsArticleRepository.AddArticle(article, dto.TagIds ?? new List<int>());

        return CreatedAtAction(nameof(GetById), new { id = article.NewsArticleId }, MapToResponseDto(article));
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] NewsArticleUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != dto.NewsArticleId)
        {
            return BadRequest(new { message = "ID in URL does not match ID in body." });
        }

        var existing = _newsArticleRepository.GetArticleById(id);
        if (existing == null)
        {
            return NotFound(new { message = $"News article with ID {id} not found." });
        }

        var article = new NewsArticle
        {
            NewsArticleId = dto.NewsArticleId,
            NewsTitle = dto.NewsTitle,
            Headline = dto.Headline,
            NewsContent = dto.NewsContent,
            NewsSource = dto.NewsSource,
            CategoryId = dto.CategoryId,
            NewsStatus = dto.NewsStatus,
            UpdatedById = dto.UpdatedById
        };

        _newsArticleRepository.UpdateArticle(article, dto.TagIds);
        return Ok(new { message = "News article updated successfully." });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        bool success = _newsArticleRepository.DeleteArticle(id, out string errorMessage);
        if (!success)
        {
            return BadRequest(new { message = errorMessage });
        }

        return Ok(new { message = "News article deleted successfully." });
    }

    private static NewsArticleResponseDto MapToResponseDto(NewsArticle n)
    {
        return new NewsArticleResponseDto
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
        };
    }
}
