using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _07_PhamToMinhHieu_Assignment01_Frontend.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}

public class UserSessionDto
{
    public short AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountEmail { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? NumericRole { get; set; }
}

public class AccountViewModel
{
    public short AccountId { get; set; }

    [Required(ErrorMessage = "Account name is required.")]
    [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters.")]
    public string AccountName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Account email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(70, ErrorMessage = "Email cannot exceed 70 characters.")]
    public string AccountEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    public int AccountRole { get; set; } = 1;

    public string? AccountPassword { get; set; }

    public string RoleName => AccountRole switch
    {
        1 => "Staff",
        2 => "Lecturer",
        _ => "Unknown"
    };
}

public class CategoryViewModel
{
    public short CategoryId { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string CategoryName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category description is required.")]
    [StringLength(250, ErrorMessage = "Category description cannot exceed 250 characters.")]
    public string CategoryDesciption { get; set; } = string.Empty;

    public short? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public bool? IsActive { get; set; } = true;
}

public class TagViewModel
{
    public int TagId { get; set; }
    public string? TagName { get; set; }
    public string? Note { get; set; }
}

public class NewsArticleViewModel
{
    [Required(ErrorMessage = "News article ID is required.")]
    [StringLength(20, ErrorMessage = "ID cannot exceed 20 characters.")]
    public string NewsArticleId { get; set; } = string.Empty;

    [StringLength(400, ErrorMessage = "Title cannot exceed 400 characters.")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Headline is required.")]
    [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters.")]
    public string Headline { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }

    public string? NewsContent { get; set; }

    [StringLength(400, ErrorMessage = "News source cannot exceed 400 characters.")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "Please select a Category.")]
    public short? CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public bool? NewsStatus { get; set; } = true;

    public short? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public short? UpdatedById { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public List<int> SelectedTagIds { get; set; } = new List<int>();
    public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
}
