namespace ContactsApp.Domain.Dto;

public record class LoginDto(
    string Email,
    string Password
);
