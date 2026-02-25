using System.Security.Claims;
using System.Text;
using ContactsApp.Data;
using ContactsApp.Domain.Dto;
using ContactsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContactsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IJwtService _jwtService;

    public AuthController(AppDbContext dbContext, IJwtService jwtService)
    {
        this._dbContext = dbContext;
        this._jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = _dbContext.Contacts.FirstOrDefault(contact => contact.Email == dto.Email);
        if (user == null) return Unauthorized("Nieprawidłowy email lub hasło.");

        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!isPasswordCorrect) return Unauthorized("Nieprawidłowy email lub hasło.");

        var jwtToken = _jwtService.GenerateJwtToken(user.Id, user.Email);

        // 4. Zwracamy obiekt JSON z tokenem
        return Ok(new { token = jwtToken });
    }

}
