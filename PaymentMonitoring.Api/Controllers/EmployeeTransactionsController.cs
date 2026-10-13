using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentMonitoring.Api.Data;
using PaymentMonitoring.Api.Models;

namespace PaymentMonitoring.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Employee")]
public class EmployeeTransactionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeeTransactionsController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetEmployeeTransactions()
    {
        var transactions = await _context.Transactions
     .Where(t => t.Status == "Pending")
     .Select(t => new EmployeeTransactionResponse
     {
         Id = t.Id,
         VerifiedAt = t.VerifiedAt,
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

    [HttpGet("history")]
    public async Task<IActionResult> GetTransactionHistory()
    {
        var transactions = await _context.Transactions
            .Where(t =>
                t.Status == "Verified" ||
                t.Status == "SubmittedToSwift")
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new EmployeeTransactionResponse
            {
                Id = t.Id,
                VerifiedAt = t.VerifiedAt,
                VerifiedByUsername = t.VerifiedByUser != null
                ? t.VerifiedByUser.Username
                : null,

                SubmittedAt = t.SubmittedAt,
                SubmittedByUsername = t.SubmittedByUser != null
                ? t.SubmittedByUser.Username
                : null,

                Amount = t.Amount,
                Currency = t.Currency,
                Provider = t.Provider,
                PayeeAccountNumber = t.PayeeAccountNumber,
                SwiftCode = t.SwiftCode,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
            })
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpPut("{id}/verify")]
    public async Task<IActionResult> VerifyTransaction(
    int id,
    VerifyTransactionRequest request)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transaction == null)
        {
            return NotFound(new
            {
                message = "Transaction not found."
            });
        }

        if (transaction.Status != "Pending")
        {
            return BadRequest(new
            {
                message = "Only pending transactions can be verified."
            });
        }

        if (transaction.PayeeAccountNumber != request.PayeeAccountNumber ||
            transaction.SwiftCode != request.SwiftCode)
        {
            return BadRequest(new
            {
                message = "The payee account number or SWIFT code does not match the transaction."
            });
        }

        var employeeUserId = User.FindFirst(
        System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (employeeUserId == null || !int.TryParse(employeeUserId, out var employeeId))
        {
            return Unauthorized();
        }

        transaction.VerifiedByUserId = employeeId;
        transaction.VerifiedAt = DateTime.UtcNow;

        transaction.Status = "Verified";

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Transaction verified successfully.",
            transactionId = transaction.Id,
            verifiedAt = transaction.VerifiedAt,
            status = transaction.Status
        });
    }

    [HttpPost("{id}/submit-to-swift")]
    public async Task<IActionResult> SubmitToSwift(int id)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transaction == null)
        {
            return NotFound(new
            {
                message = "Transaction not found."
            });
        }

        if (transaction.Status != "Verified")
        {
            return BadRequest(new
            {
                message = "Only verified transactions can be submitted to SWIFT."
            });
        }
        var employeeUserId = User.FindFirst(
        System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (employeeUserId == null || !int.TryParse(employeeUserId, out var employeeId))
        {
            return Unauthorized();
        }

        transaction.SubmittedByUserId = employeeId;
        transaction.Status = "SubmittedToSwift";
        transaction.SubmittedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Transaction submitted to SWIFT successfully.",
            transactionId = transaction.Id,
            status = transaction.Status
        });

    }
}