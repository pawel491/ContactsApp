using System;
using ContactsApp.Domain.Dto;
using ContactsApp.Domain.Entities;

namespace ContactsApp.Mappers;

public static class ContactMapper
{
    public static ContactDto ToDto(this Contact contact)
    {
        return new (contact.Email, contact.Name, contact.Surname, contact.PhoneNumber, contact.DateOfBirth, contact.Category.Name);
    }
    public static ContactListItemDto ToListItemDto(this Contact contact)
    {
        return new (contact.Name, contact.Surname, contact.Email, contact.PhoneNumber);
    }
    public static Contact ToEntity(this CreateContactDto dto, int categoryId)
    {
        return new Contact {
            Name = dto.Name, 
            Surname = dto.Surname, 
            Email = dto.Email, 
            Password = dto.Password, 
            PhoneNumber = dto.PhoneNumber, 
            DateOfBirth = dto.DateOfBirth, 
            CategoryId = categoryId
        };
    }
    public static void UpdateEntity(this UpdateContactDto dto, Contact contact, int categoryId)
    {
        contact.Name = dto.Name;
        contact.Surname = dto.Surname;
        contact.Email = dto.Email;
        contact.PhoneNumber = dto.PhoneNumber;
        contact.DateOfBirth = dto.DateOfBirth;
        contact.CategoryId = categoryId;
    }
}
