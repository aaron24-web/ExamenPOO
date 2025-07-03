using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EducationalPlatformApi.Controllers;

// DTO para el login
public class LoginDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        // --- Validación de Usuario (Simulada) ---
        // En una aplicación real, aquí validarías el usuario contra la base de datos.
        // Por ahora, aceptaremos cualquier login.
        if (login.Username == null) {
            return Unauthorized();
        }

        // --- Generación del Token ---
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Los "claims" son información sobre el usuario que viaja en el token.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, login.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(60), // El token expira en 60 minutos
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = tokenString });
    }
}