namespace ContactsApp.Domain.Dto;

public record class CreateContactDto(string Name, string Surname, string Email, string Password, string PhoneNumber, DateOnly DateOfBirth, string CategoryName);
