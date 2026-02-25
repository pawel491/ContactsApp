using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Domain.Dto;

public record class CreateContactDto(
    string Name, 
    string Surname, 
    [EmailAddress]
    string Email, 
    [MinLength(8, ErrorMessage = "The password must be at least 8 characters long.")]
    string Password, 
    [Phone]
    string PhoneNumber, 
    DateOnly DateOfBirth, 
    string CategoryName,
    string? SubcategoryName,
    string? CustomSubcategory);
