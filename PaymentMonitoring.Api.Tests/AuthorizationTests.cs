
using Microsoft.AspNetCore.Hosting;
using System.Net;
using PaymentMonitoring.Api.Models;
using PaymentMonitoring.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentMonitoring.Api.Data;

namespace PaymentMonitoring.Api.Tests;

public class AuthorizationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthorizationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["Jwt:Key"] = "TestOnlySigningKeyForAutomatedTests2026!",
                        ["Jwt:Issuer"] = "PaymentMonitoring.Api",
                        ["Jwt:Audience"] = "PaymentMonitoring.Client"
                    });
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ApplicationDbContext>();
                services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
                services.RemoveAll<DbContextOptions>();
                services.RemoveAll<Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptionsConfiguration<ApplicationDbContext>>();

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("AuthorizationTestsDb"));
            });
        });
    }

    [Fact]
    public async Task EmployeeTransactions_WithoutAuthentication_ReturnsUnauthorized()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/EmployeeTransactions");

        Assert.Equal(
            System.Net.HttpStatusCode.Unauthorized,
            response.StatusCode);
    }


    [Fact]
    public async Task CustomerAccessingEmployeeTransactions_ReturnsForbidden()
    {
        using var client = _factory.CreateClient();

        var jwtService = _factory.Services.GetRequiredService<JwtService>();

        var customer = new User
        {
            Id = 1,
            Username = "testcustomer",
            AccountNumber = "1234567890",
            Role = "Customer",
            IsActive = true
        };

        var token = jwtService.GenerateToken(customer);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        var response = await client.GetAsync("/api/EmployeeTransactions");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }


    [Fact]
    public async Task Transactions_WithoutAuthentication_ReturnsUnauthorized()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/Transactions");

        Assert.Equal(
            System.Net.HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task EmployeeAccessingEmployeeTransactions_ReturnsSuccess()
    {
        using var client = _factory.CreateClient();

        var jwtService = _factory.Services.GetRequiredService<JwtService>();

        var employee = new User
        {
            Id = 2,
            Username = "testemployee",
            AccountNumber = "9876543210",
            Role = "Employee",
            IsActive = true
        };

        var token = jwtService.GenerateToken(employee);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        var response = await client.GetAsync("/api/EmployeeTransactions");

        Assert.Equal(
            System.Net.HttpStatusCode.OK,
            response.StatusCode);
    }


}

