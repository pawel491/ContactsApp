using ContactsApp.Data;
using ContactsApp.Domain.Dto;
using ContactsApp.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public CategoryController(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _dbContext.Categories
            .Include(c => c.Subcategories)
            .ToListAsync();
        
        var dtos = categories.Select(c => c.ToDto());
        return Ok(dtos);
    }
}

