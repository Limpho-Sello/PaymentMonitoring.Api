
using System.ComponentModel.DataAnnotations;
using PaymentMonitoring.Api.Models;

namespace PaymentMonitoring.Api.Tests;

public class LoginRequestTests
{
    private static IList<ValidationResult> ValidateRequest(LoginRequest request)
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

    private static LoginRequest ValidRequest()
    {
        return new LoginRequest
        {
            Username = "testuser",
            AccountNumber = "1234567890",
            Password = "TestPassword123!"
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
    public void UsernameWithInvalidCharacters_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.Username = "test user!";

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void AccountNumberWithLetters_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.AccountNumber = "12345ABCDE";

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void PasswordTooShort_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.Password = "Short7!";

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void PasswordTooLong_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.Password = new string('A', 129);

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void EmptyUsername_ShouldFailValidation()
    {
        var request = ValidRequest();
        request.Username = string.Empty;

        var results = ValidateRequest(request);

        Assert.NotEmpty(results);
    }
}
