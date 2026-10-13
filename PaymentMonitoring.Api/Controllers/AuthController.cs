using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using PaymentMonitoring.Api.Data;
using PaymentMonitoring.Api.Models;
using PaymentMonitoring.Api.Services;
namespace PaymentMonitoring.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly JwtService _jwtService;

    public AuthController(
    ApplicationDbContext context,
    PasswordService passwordService,
    JwtService jwtService)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    [EnableRateLimiting("LoginRateLimit")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Username == request.Username &&
                u.AccountNumber == request.AccountNumber &&
                u.IsActive);

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid username, account number, or password."
            });
        }

        var passwordValid = _passwordService.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            return Unauthorized(new
            {
                message = "Invalid username, account number, or password."
            });
        }

        var token = _jwtService.GenerateToken(user);

        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromMinutes(30)
        });

        return Ok(new
        {
            message = "Login successful.",
            username = user.Username,
            role = user.Role
        });


    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            username = User.Identity?.Name,
            role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");

        return Ok(new
        {
            message = "Logout successful."
        });
    }
}