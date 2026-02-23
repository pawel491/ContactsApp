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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Category>()
            .HasData(
                new Category { Id = 1, Name = "służbowy"},
                new Category { Id = 2, Name = "prywatny"},
                new Category { Id = 3, Name = "inny" }
            );

        modelBuilder.Entity<Contact>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Contact>()
            .HasIndex(c => c.Email)
            .IsUnique();
        modelBuilder.Entity<Contact>()
            .HasData(
                new Contact { Id = 1, Email = "test@aa.aa", Name = "Test", Surname = "Testowy", Password = "somepassword", PhoneNumber = "+48123123123", DateOfBirth = new DateOnly(2000,1,1), CategoryId = 1},
                new Contact { Id = 2, Email = "testtest@aa.aa", Name = "TestDrugi", Surname = "TestowyDwa", Password = "somepasswordlonger", PhoneNumber = "+48999999999", DateOfBirth = new DateOnly(2005,3,3), CategoryId = 2}
            );


    }
}
