using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using _07_PhamToMinhHieu_Assignment01_BackEnd.BusinessObjects;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;
using _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoryResponseDto>> GetAll([FromQuery] string? search)
    {
        var categories = string.IsNullOrWhiteSpace(search)
            ? _categoryRepository.GetCategories()
            : _categoryRepository.SearchCategories(search);

        var response = categories.Select(c => new CategoryResponseDto
        {
            CategoryId = c.CategoryId,
            CategoryName = c.CategoryName,
            CategoryDesciption = c.CategoryDesciption,
            ParentCategoryId = c.ParentCategoryId,
            ParentCategoryName = c.ParentCategory?.CategoryName,
            IsActive = c.IsActive
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public ActionResult<CategoryResponseDto> GetById(short id)
    {
        var category = _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found." });
        }

        return Ok(new CategoryResponseDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            CategoryDesciption = category.CategoryDesciption,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryName = category.ParentCategory?.CategoryName,
            IsActive = category.IsActive
        });
    }

    [HttpPost]
    public IActionResult Create([FromBody] CategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = new Category
        {
            CategoryName = dto.CategoryName,
            CategoryDesciption = dto.CategoryDesciption,
            ParentCategoryId = dto.ParentCategoryId,
            IsActive = dto.IsActive ?? true
        };

        _categoryRepository.AddCategory(category);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, new CategoryResponseDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            CategoryDesciption = category.CategoryDesciption,
            ParentCategoryId = category.ParentCategoryId,
            IsActive = category.IsActive
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(short id, [FromBody] CategoryUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != dto.CategoryId)
        {
            return BadRequest(new { message = "ID in URL does not match ID in body." });
        }

        var existing = _categoryRepository.GetCategoryById(id);
        if (existing == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found." });
        }

        if (dto.ParentCategoryId.HasValue && dto.ParentCategoryId.Value == id)
        {
            return BadRequest(new { message = "Category cannot be its own parent." });
        }

        existing.CategoryName = dto.CategoryName;
        existing.CategoryDesciption = dto.CategoryDesciption;
        existing.ParentCategoryId = dto.ParentCategoryId;
        existing.IsActive = dto.IsActive;

        _categoryRepository.UpdateCategory(existing);
        return Ok(new { message = "Category updated successfully." });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(short id)
    {
        bool success = _categoryRepository.DeleteCategory(id, out string errorMessage);
        if (!success)
        {
            return BadRequest(new { message = errorMessage });
        }

        return Ok(new { message = "Category deleted successfully." });
    }
}
