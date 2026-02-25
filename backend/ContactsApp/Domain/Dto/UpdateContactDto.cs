using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Domain.Dto;

public record class UpdateContactDto(
    string Name, 
    string Surname, 
    [EmailAddress]
    string Email, 
    [Phone]
    string PhoneNumber,
    DateOnly DateOfBirth, 
    string CategoryName,
    string? SubcategoryName,
    string? CustomSubcategory
    );
