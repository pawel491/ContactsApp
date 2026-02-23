namespace ContactsApp.Domain.Dto;

public record class UpdateContactDto(string Name, string Surname, string Email, string PhoneNumber, DateOnly DateOfBirth, string CategoryName);
