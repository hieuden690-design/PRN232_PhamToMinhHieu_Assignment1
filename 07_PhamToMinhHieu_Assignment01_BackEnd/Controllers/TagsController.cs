using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using _07_PhamToMinhHieu_Assignment01_BackEnd.DTOs;
using _07_PhamToMinhHieu_Assignment01_BackEnd.Repositories;

namespace _07_PhamToMinhHieu_Assignment01_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagRepository _tagRepository;

    public TagsController(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TagDto>> GetAll()
    {
        var tags = _tagRepository.GetTags();
        var response = tags.Select(t => new TagDto
        {
            TagId = t.TagId,
            TagName = t.TagName,
            Note = t.Note
        });
        return Ok(response);
    }
}
