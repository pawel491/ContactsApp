using System;

namespace ContactsApp.Domain.Dto;

public record class ContactListItemDto(string Name, string Surname, string Email, string PhoneNumber);
