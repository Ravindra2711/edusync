using EduSync.Infrastructure.Data;
using EduSync.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IConfiguration _config;

    public AuthController(DataContext context,
                          IPasswordHasher<User> hasher,
                          IConfiguration config)
    {
        _context = context;
        _hasher = hasher;
        _config = config;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest req)
    {
        if (_context.Users.Any(u => u.Email == req.Email))
            return Conflict("Email already in use.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = req.Name,
            Email = req.Email,
            Role = "Student"
        };
        user.PasswordHash = Encoding.UTF8.GetBytes(_hasher.HashPassword(user, req.Password));

        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Me), new { }, new { user.Id, user.Name, user.Email });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // existing login with validation using hasher
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);
        if (user is null) return Unauthorized();

        var result = _hasher.VerifyHashedPassword(user, Encoding.UTF8.GetString(user.PasswordHash), request.Password);
        if (result != PasswordVerificationResult.Success) return Unauthorized();

        // generate JWT token (same as before)
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _context.Users.Find(Guid.Parse(userId));
        return user is not null
            ? Ok(new { user.Id, user.Name, user.Email, user.Role })
            : NotFound();
    }
}

public class RegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}