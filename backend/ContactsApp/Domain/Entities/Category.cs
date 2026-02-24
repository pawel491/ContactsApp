using System;

namespace ContactsApp.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<Contact> Contacts { get; set; } = new();

    public List<Subcategory> Subcategories { get; set; } = new();
}
