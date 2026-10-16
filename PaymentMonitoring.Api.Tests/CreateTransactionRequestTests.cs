
using System.ComponentModel.DataAnnotations;
using PaymentMonitoring.Api.Models;

namespace PaymentMonitoring.Api.Tests;

public class CreateTransactionRequestTests
{
    private static IList<ValidationResult> ValidateRequest(
        CreateTransactionRequest request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(
            request,
            context,
            results,
            validateAllProperties: true);

        return results;
    }

    private static CreateTransactionRequest ValidRequest()
    {
        return new CreateTransactionRequest
        {
            Amount = 1500m,
            Currency = "ZAR",
            Provider = "SWIFT",
            PayeeAccountNumber = "1234567890",
            SwiftCode = "ABSAZAJJ"
        };
    }

    [Fact]
    public void ValidRequest_ShouldPassValidation()
    {
        var request = ValidRequest();

        var results = ValidateRequest(request);

        Assert.Empty(results);
    }

    [Fact]
    public void ZeroAmount_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.Amount = 0m;

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void UnsupportedCurrency_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.Currency = "USDX";

        var results = ValidateRequest(request);

        Assert.Contains(results, result =>
            result.MemberNames.Contains(nameof(request.Currency)));
    }

    [Fact]
    public void InvalidPayeeAccountNumber_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.PayeeAccountNumber = "12345ABC";

        var results = ValidateRequest(request);

        Assert.Contains(results, result =>
            result.MemberNames.Contains(nameof(request.PayeeAccountNumber)));
    }

    [Fact]
    public void InvalidSwiftCode_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.SwiftCode = "ABC123";

        var results = ValidateRequest(request);

        Assert.Contains(results, result =>
            result.MemberNames.Contains(nameof(request.SwiftCode)));
    }


    [Fact]
    public void MinimumAllowedAmount_ShouldPassValidation()
    {
        var request = ValidRequest();
        request.Amount = 0.01m;

        var results = ValidateRequest(request);

        Assert.DoesNotContain(results, result =>
            result.MemberNames.Contains(nameof(request.Amount)));
    }

    [Fact]
    public void AmountAboveMaximum_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.Amount = 1000000000000.00m;

        var results = ValidateRequest(request);

        Assert.Contains(results, result =>
            result.MemberNames.Contains(nameof(request.Amount)));
    }

}
