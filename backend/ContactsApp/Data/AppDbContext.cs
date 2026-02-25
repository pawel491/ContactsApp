using System;
using ContactsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace ContactsApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Subcategory> Subcategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Category>()
            .HasData(
                new Category { Id = 1, Name = "służbowy"}, // only this category will have subcategories
                new Category { Id = 2, Name = "prywatny"},
                new Category { Id = 3, Name = "inny" }
            );

        modelBuilder.Entity<Subcategory>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Subcategory>()
            .HasData(
                new Subcategory { Id = 1, Name = "szef", CategoryId = 1},
                new Subcategory { Id = 2, Name = "klient", CategoryId = 1},
                new Subcategory { Id = 3, Name = "współpracownik", CategoryId = 1}
            );

        modelBuilder.Entity<Contact>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Contact>()
            .HasIndex(c => c.Email)
            .IsUnique();

    }
}
