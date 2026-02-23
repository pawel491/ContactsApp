namespace ContactsApp.Domain.Dto;

public record class ContactDto(string Email, string Name, string Surname, string PhoneNumber, DateOnly DateOfBirth, string Category);

