using System.ComponentModel.DataAnnotations;

namespace PaymentMonitoring.Api.Models;

public class VerifyTransactionRequest
{
    [Required]
    [RegularExpression(
        @"^\d{10,20}$",
        ErrorMessage = "Payee account number must contain 10 to 20 digits.")]
    public string PayeeAccountNumber { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^[A-Z]{8}([A-Z0-9]{3})?$",
        ErrorMessage = "SWIFT code must be 8 or 11 uppercase letters/numbers.")]
    public string SwiftCode { get; set; } = string.Empty;
}