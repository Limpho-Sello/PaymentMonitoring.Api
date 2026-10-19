
using System.ComponentModel.DataAnnotations;
using PaymentMonitoring.Api.Models;

namespace PaymentMonitoring.Api.Tests;

public class VerifyTransactionRequestTests
{
    private static IList<ValidationResult> ValidateRequest(
        VerifyTransactionRequest request)
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

    private static VerifyTransactionRequest ValidRequest()
    {
        return new VerifyTransactionRequest
        {
            PayeeAccountNumber = "1234567890",
            SwiftCode = "ABSAZAJJ"
        };
    }

    [Fact]
    public void ValidRequest_ShouldPassValidation()
    {
        var results = ValidateRequest(ValidRequest());

        Assert.Empty(results);
    }

    [Fact]
    public void EmptyPayeeAccountNumber_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.PayeeAccountNumber = string.Empty;

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void PayeeAccountNumberWithLetters_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.PayeeAccountNumber = "12345ABCDE";

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void PayeeAccountNumberTooShort_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.PayeeAccountNumber = "123456789";

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void EmptySwiftCode_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.SwiftCode = string.Empty;

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void LowercaseSwiftCode_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.SwiftCode = "absazajj";

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }
}
