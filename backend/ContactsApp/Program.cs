using ContactsApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var frontendUrl = builder.Configuration.GetValue<string>("Frontend:Url");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var AllowFrontendOrigins = "_allowFrontendOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowFrontendOrigins,
    policy =>
    {
        policy.WithOrigins(frontendUrl)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseCors(AllowFrontendOrigins);

app.MapControllers();
app.Run();
