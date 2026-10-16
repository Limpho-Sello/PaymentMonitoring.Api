
using PaymentMonitoring.Api.Services;

namespace PaymentMonitoring.Api.Tests;

public class PasswordServiceTests
{
    [Fact]
    public void HashPassword_ShouldReturnDifferentValueFromPlainText()
    {
        // Arrange
        var passwordService = new PasswordService();
        var password = "TestPassword123!";

        // Act
        var hashedPassword = passwordService.HashPassword(password);

        // Assert
        Assert.NotEqual(password, hashedPassword);
        Assert.False(string.IsNullOrWhiteSpace(hashedPassword));
    }


    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
    {
        // Arrange
        var passwordService = new PasswordService();
        var password = "TestPassword123!";
        var hashedPassword = passwordService.HashPassword(password);

        // Act
        var result = passwordService.VerifyPassword(password, hashedPassword);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
    {
        // Arrange
        var passwordService = new PasswordService();
        var hashedPassword = passwordService.HashPassword("TestPassword123!");

        // Act
        var result = passwordService.VerifyPassword("WrongPassword123!", hashedPassword);

        // Assert
        Assert.False(result);
    }

}
