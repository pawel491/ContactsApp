using System;
using ContactsApp.Data;
using ContactsApp.Domain.Entities;

namespace ContactsApp.Services;

public static class DbInitializer
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
        var config = serviceScope.ServiceProvider.GetService<IConfiguration>();

        dbContext!.Database.EnsureCreated(); // Upewnia się, że baza i tabele istnieją

        if (!dbContext.Contacts.Any())
        {
            var adminEmail = config["SEED_ADMIN_EMAIL"] ?? "admin@aa.aa";
            var adminPass = config["SEED_ADMIN_PASSWORD"] ?? "adminpass123";

            dbContext.Contacts.Add(new Contact
            {
                Email = adminEmail,
                Name = "Admin",
                Surname = "Systemowy",
                Password = BCrypt.Net.BCrypt.HashPassword(adminPass),
                PhoneNumber = "+48000000000",
                DateOfBirth = new DateOnly(2000, 1, 1),
                CategoryId = 2
            });

            dbContext.SaveChanges();
        }
    }
}
