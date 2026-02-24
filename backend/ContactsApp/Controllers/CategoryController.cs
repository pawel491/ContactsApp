using ContactsApp.Data;
using ContactsApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
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
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _dbContext.Categories.Select(
            category => new
            {
                category.Id,
                category.Name,
                Subcategories = category.Subcategories.Select(subcat => new { subcat.Id, subcat.Name}).ToList()
            }
        ).ToListAsync();
        return Ok(categories);
    }
}

