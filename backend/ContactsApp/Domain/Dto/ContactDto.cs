namespace ContactsApp.Domain.Dto;

public record class ContactDto(
    int Id,
    string Email, 
    string Name, 
    string Surname,
    string PhoneNumber, 
    DateOnly DateOfBirth, 
    string CategoryName,
    string? SubcategoryName,
    string? CustomSubcategory);

