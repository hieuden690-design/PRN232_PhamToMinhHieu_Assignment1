using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;

public class TagDto
{
    public int TagId { get; set; }
    public string? TagName { get; set; }
    public string? Note { get; set; }
}

public class NewsArticleCreateDto
{
    [Required(ErrorMessage = "NewsArticleId is required.")]
    [StringLength(20, ErrorMessage = "Article ID cannot exceed 20 characters.")]
    public string NewsArticleId { get; set; } = string.Empty;

    [StringLength(400, ErrorMessage = "Title cannot exceed 400 characters.")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Headline is required.")]
    [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters.")]
    public string Headline { get; set; } = string.Empty;

    public string? NewsContent { get; set; }

    [StringLength(400, ErrorMessage = "News source cannot exceed 400 characters.")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public short? CategoryId { get; set; }

    public bool? NewsStatus { get; set; } = true;

    public short? CreatedById { get; set; }

    public List<int> TagIds { get; set; } = new List<int>();
}

public class NewsArticleUpdateDto
{
    [Required(ErrorMessage = "NewsArticleId is required.")]
    [StringLength(20, ErrorMessage = "Article ID cannot exceed 20 characters.")]
    public string NewsArticleId { get; set; } = string.Empty;

    [StringLength(400, ErrorMessage = "Title cannot exceed 400 characters.")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Headline is required.")]
    [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters.")]
    public string Headline { get; set; } = string.Empty;

    public string? NewsContent { get; set; }

    [StringLength(400, ErrorMessage = "News source cannot exceed 400 characters.")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public short? CategoryId { get; set; }

    public bool? NewsStatus { get; set; }

    public short? UpdatedById { get; set; }

    public List<int> TagIds { get; set; } = new List<int>();
}

public class NewsArticleResponseDto
{
    public string NewsArticleId { get; set; } = string.Empty;
    public string? NewsTitle { get; set; }
    public string Headline { get; set; } = string.Empty;
    public DateTime? CreatedDate { get; set; }
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public short? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public bool? NewsStatus { get; set; }
    public short? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public short? UpdatedById { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<TagDto> Tags { get; set; } = new List<TagDto>();
}
