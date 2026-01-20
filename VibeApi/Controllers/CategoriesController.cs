using Microsoft.AspNetCore.Mvc;
using VibeApi.Models;

namespace VibeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private static readonly List<Category> _categories = new();
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        return Ok(_categories);
    }

    [HttpGet("{id}")]
    public ActionResult<Category> GetCategory(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpPost]
    public ActionResult<Category> CreateCategory(CategoryDto categoryDto)
    {
        var category = new Category
        {
            Id = _nextId++,
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            IsActive = categoryDto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _categories.Add(category);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id, CategoryDto categoryDto)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();

        category.Name = categoryDto.Name;
        category.Description = categoryDto.Description;
        category.IsActive = categoryDto.IsActive;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();

        _categories.Remove(category);
        return NoContent();
    }
}