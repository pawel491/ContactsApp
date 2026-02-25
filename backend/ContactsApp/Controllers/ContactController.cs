using System.Collections;
using ContactsApp.Data;
using ContactsApp.Domain.Dto;
using ContactsApp.Domain.Entities;
using ContactsApp.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public ContactController(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ContactListItemDto>>> GetContacts()
    {
        var contacts = await _dbContext.Contacts
            .Include(contact => contact.Category)
            .Include(contact => contact.Subcategory)
            .ToListAsync();

        var dtos = contacts.Select(contact => contact.ToListItemDto());
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContact(int id)
    {
        var contact = await _dbContext.Contacts
            .Include(contact => contact.Category)
            .Include(contact => contact.Subcategory)
            .FirstOrDefaultAsync(contact => contact.Id == id);

        return contact != null ? Ok(contact.ToDto()) : NotFound(); 
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact([FromBody] CreateContactDto dto)
    {
        if (await _dbContext.Contacts.AnyAsync(contact => contact.Email == dto.Email))
            return BadRequest("Contact with given email address already exists.");

        var category = await _dbContext.Categories.FirstOrDefaultAsync(category => category.Name.ToLower() == dto.CategoryName.ToLower());
        if (category == null) return BadRequest("Category doesn't exist");

        int? subcategoryId = null;
        Subcategory? subcategory = null;

        if (!string.IsNullOrEmpty(dto.SubcategoryName))
        {
            //check if given subcategory exists for given category
            subcategory = await _dbContext.Subcategories
                .FirstOrDefaultAsync(subcat => subcat.Name.ToLower() == dto.SubcategoryName.ToLower() && subcat.CategoryId == category.Id);

            if(subcategory == null) return BadRequest("Subcategory doesn't exist for given category");
            subcategoryId = subcategory.Id;
        }
        var contact = dto.ToEntity(category.Id, subcategoryId);
        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();

        // to make sure .ToDto() won't fail
        contact.Category = category;
        if(subcategory != null)
        {
            contact.Subcategory = subcategory;
        }

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateContact([FromBody] UpdateContactDto dto, int id)
    {
        if (await _dbContext.Contacts.AnyAsync(contact => contact.Email == dto.Email && contact.Id != id))
        {
            return BadRequest("Another contact is already using given email address");
        }

        var contact = await _dbContext.Contacts.FindAsync(id);
        if(contact == null) return NotFound();

        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.CategoryName.ToLower());
        if(category == null) return BadRequest("Category doesn't exist");

        int? subcategoryId = null;
        if (!string.IsNullOrEmpty(dto.SubcategoryName))
        {
            var subcategory = await _dbContext.Subcategories
                .FirstOrDefaultAsync(subcat => subcat.Name.ToLower() == dto.SubcategoryName.ToLower() && subcat.CategoryId == category.Id);

            if(subcategory == null) return BadRequest("Subcategory doesn't exist for given category");
            
            subcategoryId = subcategory.Id;
        }

        dto.UpdateEntity(contact, category.Id, subcategoryId);
        await _dbContext.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteContact(int id)
    {
        var contact = await _dbContext.Contacts.FirstOrDefaultAsync(contact => contact.Id == id);
        if(contact == null) return NotFound();

        _dbContext.Contacts.Remove(contact);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
