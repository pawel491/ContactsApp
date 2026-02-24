using System;

namespace ContactsApp.Domain.Dto;

public record class ContactListItemDto(
    int Id,
    string Name, 
    string Surname, 
    string Email, 
    string PhoneNumber);
