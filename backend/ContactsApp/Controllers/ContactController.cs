using System.Collections;
using ContactsApp.Data;
using ContactsApp.Domain.Dto;
using ContactsApp.Domain.Entities;
using ContactsApp.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public ContactController(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
    {
        var contacts = await _dbContext.Contacts
            .Include(contact => contact.Category)
            .ToListAsync();

        var dtos = contacts.Select(contact => contact.ToListItemDto());
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDto>> GetContact(int id)
    {
        var contact = await _dbContext.Contacts
            .Include(contact => contact.Category)
            .FirstOrDefaultAsync(contact => contact.Id == id);

        return contact != null ? Ok(contact.ToDto()) : NotFound(); 
    }

    [HttpPost]
    public async Task<ActionResult<ContactDto>> CreateContact([FromBody] CreateContactDto dto)
    {
        if (await _dbContext.Contacts.AnyAsync(contact => contact.Email == dto.Email))
        {
            return BadRequest("Contact with given email address already exists.");
        }

        var category = await _dbContext.Categories.FirstOrDefaultAsync(category => category.Name.ToLower() == dto.CategoryName.ToLower());
        if (category == null)
        {
            // create a new category
            category = new Category { Name = dto.CategoryName };
            _dbContext.Categories.Add(category);
            //save now so Id is generated
            await _dbContext.SaveChangesAsync();
        }
        var contact = dto.ToEntity(category.Id);
        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();

        // to make sure .ToDto() won't fail
        contact.Category = category;

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateContact([FromBody] UpdateContactDto dto, int id)
    {
        if (await _dbContext.Contacts.AnyAsync(contact => contact.Email == dto.Email && contact.Id != id))
        {
            return BadRequest("Another contact is already using given email address");
        }

        var contact = await _dbContext.Contacts.Include(contact => contact.Category).FirstOrDefaultAsync(contact => contact.Id == id);
        if(contact == null) return NotFound();

        int newCategoryId = contact.CategoryId;

        // if categoryName changed then newCategoryId need to be changed
        if(contact.Category.Name.ToLower() != dto.CategoryName.ToLower())
        {
            // check if already exists
            var category = await _dbContext.Categories.FirstOrDefaultAsync(category => category.Name.ToLower() == dto.CategoryName.ToLower());
            if(category == null)
            {
                // create new category
                category = new Category { Name = dto.CategoryName };
                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();
            }
            newCategoryId = category.Id;
        }
        dto.UpdateEntity(contact, newCategoryId);
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
