

namespace ContactsApp.Domain.Entities;

public class Contact
{
    //unique
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}