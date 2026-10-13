using System.ComponentModel.DataAnnotations;

namespace PaymentMonitoring.Api.Models;

public class CreateTransactionRequest
{
    [Range(0.01, 999999999999.99)]
    public decimal Amount { get; set; }

    public static readonly string[] AllowedCurrencies =
    {
        "ZAR",
        "LSL",
        "USD",
        "EUR",
        "GBP"
    };

    [Required]
    [RegularExpression(
      @"^(LSL|ZAR|USD|EUR|GBP)$",
      ErrorMessage = "Currency must be one of: LSL, ZAR, USD, EUR, GBP.")]
    public string Currency { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
    @"^SWIFT$",
    ErrorMessage = "Provider must be SWIFT.")]
    public string Provider { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
    @"^\d{10,20}$",
    ErrorMessage = "Payee account number must contain only digits and be between 10 and 20 digits.")]
    public string PayeeAccountNumber { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^[A-Z]{8}([A-Z0-9]{3})?$",
        ErrorMessage = "SWIFT code must be 8 or 11 uppercase letters/numbers.")]
    public string SwiftCode { get; set; } = string.Empty;

    public static readonly string[] AllowedProviders =
    {
        "SWIFT"
    };
}