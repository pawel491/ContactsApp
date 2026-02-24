using ContactsApp.Domain.Dto;
using ContactsApp.Domain.Entities;

namespace ContactsApp.Mappers;

public static class ContactMapper
{
    public static ContactDto ToDto(this Contact contact)
    {
        return new (
            contact.Id,
            contact.Email, 
            contact.Name, 
            contact.Surname, 
            contact.PhoneNumber, 
            contact.DateOfBirth, 
            contact.Category.Name, 
            contact.Subcategory?.Name, 
            contact.CustomSubcategory);
    }
    public static ContactListItemDto ToListItemDto(this Contact contact)
    {
        return new (
            contact.Id, 
            contact.Name, 
            contact.Surname, 
            contact.Email, 
            contact.PhoneNumber);
    }
    public static Contact ToEntity(this CreateContactDto dto, int categoryId, int? subcategoryId)
    {
        return new Contact {
            Name = dto.Name, 
            Surname = dto.Surname, 
            Email = dto.Email, 
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password), 
            PhoneNumber = dto.PhoneNumber, 
            DateOfBirth = dto.DateOfBirth, 
            CategoryId = categoryId,

            SubcategoryId = subcategoryId,
            CustomSubcategory = dto.CustomSubcategory
        };
    }
    public static void UpdateEntity(this UpdateContactDto dto, Contact contact, int categoryId, int? subcategoryId)
    {
        contact.Name = dto.Name;
        contact.Surname = dto.Surname;
        contact.Email = dto.Email;
        contact.PhoneNumber = dto.PhoneNumber;
        contact.DateOfBirth = dto.DateOfBirth;
        contact.CategoryId = categoryId;

        contact.SubcategoryId = subcategoryId;
        contact.CustomSubcategory = dto.CustomSubcategory;
    }
}
