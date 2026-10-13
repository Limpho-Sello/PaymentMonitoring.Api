using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PaymentMonitoring.Api.Data;
using PaymentMonitoring.Api.Models;

namespace PaymentMonitoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerTransactionResponse>>> GetTransactions()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            if (!int.TryParse(userId, out var userIdValue))
            {
                return Unauthorized();
            }

            var transactions = await _context.Transactions
            .Where(t => t.UserId == userIdValue)
            .Select(t => new CustomerTransactionResponse
            {
                Id = t.Id,
                Amount = t.Amount,
                Currency = t.Currency,
                Provider = t.Provider,
                PayeeAccountNumber = t.PayeeAccountNumber,
                SwiftCode = t.SwiftCode,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                SubmittedAt = t.SubmittedAt
            })
            .ToListAsync();

            return Ok(transactions);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction(
            CreateTransactionRequest request)
        {
            if (!CreateTransactionRequest.AllowedProviders.Contains(
            request.Provider,
            StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    message = "The selected provider is not supported."
                });
            }

            if (!CreateTransactionRequest.AllowedCurrencies.Contains(
                    request.Currency,
                    StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    message = "The selected currency is not supported."
                });
            }
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null || !int.TryParse(userId, out var userIdValue))
            {
                return Unauthorized();
            }

            var transaction = new Transaction
            {
                UserId = userIdValue,
                Amount = request.Amount,
                Currency = request.Currency,
                Provider = request.Provider,
                PayeeAccountNumber = request.PayeeAccountNumber,
                SwiftCode = request.SwiftCode,
                Status = "Pending"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var response = new CreatedTransactionResponse
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                Provider = transaction.Provider,
                PayeeAccountNumber = transaction.PayeeAccountNumber,
                SwiftCode = transaction.SwiftCode,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetTransactions),
                new { id = transaction.Id },
                response);
        }
    }
}