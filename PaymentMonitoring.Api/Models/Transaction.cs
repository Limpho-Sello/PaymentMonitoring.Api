namespace PaymentMonitoring.Api.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        // The user who owns this transaction
        public int UserId { get; set; }

        public User? VerifiedByUser { get; set; }

        public User? SubmittedByUser { get; set; }

        public int? VerifiedByUserId { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public int? SubmittedByUserId { get; set; }

        public DateTime? SubmittedAt { get; set; }


        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string Provider { get; set; } = string.Empty;
        public string PayeeAccountNumber { get; set; } = string.Empty;

        public string SwiftCode { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation property
        public User? User { get; set; }
    }
}