
namespace PaymentMonitoring.Api.Models;

public class CreatedTransactionResponse
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = string.Empty;

    public string Provider { get; set; } = string.Empty;

    public string PayeeAccountNumber { get; set; } = string.Empty;

    public string SwiftCode { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}