using System.ComponentModel.DataAnnotations;

namespace PaymentMonitoring.Api.Models;

public class LoginRequest
{
    [Required]
    [RegularExpression(
        @"^[A-Za-z0-9_]{3,50}$",
        ErrorMessage = "Username contains invalid characters.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^\d{10,20}$",
        ErrorMessage = "Account number must contain only digits.")]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^.{8,128}$",
        ErrorMessage = "Password must be between 8 and 128 characters.")]
    public string Password { get; set; } = string.Empty;
}