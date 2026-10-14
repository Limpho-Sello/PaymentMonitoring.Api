# Payment Monitoring API

A RESTful Web API built with ASP.NET Core for monitoring international payment transactions through a simulated payment-processing workflow. The project demonstrates secure authentication, role-based authorization, transaction validation, and audit tracking.

## Features

- **Authentication:** Login and logout endpoints with JWT-based authentication using an HttpOnly cookie.
- **Role-based access control:** Separate permissions for customers and bank employees.
- **Transaction management:** Customers can create and view their own payment transactions.
- **Employee review workflow:** Employees can review pending transactions and verify payment details.
- **Payment submission simulation:** Verified transactions can be marked as submitted to a simulated SWIFT workflow. No real SWIFT network integration is performed.
- **Audit trail:** Records which employee verified and submitted a transaction, together with timestamps.
- **Input validation:** Validates payment amounts, supported currencies, provider values, beneficiary account numbers, and SWIFT/BIC code formats.
- **Data persistence:** Uses Entity Framework Core and SQL Server LocalDB.
- **Configuration security:** Stores the JWT signing key in .NET User Secrets during local development rather than committing it to source control.

## Technology Stack

- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- SQL Server LocalDB
- JWT authentication
- REST API
- Git and GitHub

## Payment Workflow

1. A customer signs in and creates a payment transaction.
2. The transaction enters the `Pending` state.
3. An authenticated employee reviews the payment details.
4. The employee verifies the beneficiary account number and SWIFT/BIC code.
5. A successfully verified transaction moves to `Verified`.
6. The employee can submit the verified transaction to the simulated SWIFT workflow, changing its status to `SubmittedToSwift`.
7. The system records the employee responsible and relevant timestamps for audit purposes.

## Authorization

| Role | Permissions |
|---|---|
| Customer | Create transactions and view their own transactions |
| Employee | View pending transactions, verify payments, submit verified transactions, and view transaction history |

## API Endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/Auth/login` | Authenticate a user |
| GET | `/api/Auth/me` | Retrieve the authenticated user's information |
| POST | `/api/Auth/logout` | Log out |
| GET | `/api/Transactions` | Retrieve the current customer's transactions |
| POST | `/api/Transactions` | Create a transaction |
| GET | `/api/EmployeeTransactions` | Retrieve pending transactions |
| GET | `/api/EmployeeTransactions/history` | View reviewed and submitted transaction history |
| PUT | `/api/EmployeeTransactions/{id}/verify` | Verify a pending transaction |
| POST | `/api/EmployeeTransactions/{id}/submit-to-swift` | Submit a verified transaction to the simulated workflow |

## Getting Started

### Prerequisites

- Visual Studio with support for .NET 10
- .NET 10 SDK
- SQL Server LocalDB
- Git

### 1. Clone the repository

```bash
git clone https://github.com/Limpho-Sello/PaymentMonitoring.Api.git
cd PaymentMonitoring.Api
```

### 2. Configure the JWT signing key

Configure the JWT key using .NET User Secrets. From the directory containing `PaymentMonitoring.Api.csproj`, run:

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "REPLACE_WITH_A_STRONG_RANDOM_SECRET"
```

Use a strong, randomly generated signing key. Never commit the actual key to GitHub.

The `Jwt:Issuer` and `Jwt:Audience` values are configured in `appsettings.json`.

### 3. Configure the database

Check the `DefaultConnection` setting in `appsettings.json`. The default configuration uses SQL Server LocalDB with Windows trusted authentication.

Ensure LocalDB is installed and available on your machine.

### 4. Apply database migrations

From the directory containing `PaymentMonitoring.Api.csproj`, run:

```bash
dotnet ef database update
```

If the Entity Framework command is unavailable, install the EF Core CLI tool:

```bash
dotnet tool install --global dotnet-ef
```

### 5. Run the API

From the project directory, run:

```bash
dotnet run
```

Alternatively, open `PaymentMonitoring.Api.slnx` in Visual Studio and run the API using your configured launch profile.

Use the local URL shown in the terminal or launch settings to access the API.

## Development Notes

- Development seed accounts are for local testing only.
- Passwords are stored as hashes rather than plaintext.
- The payment submission step simulates a workflow; it does not send payments through a real banking network.
- Do not use this project to process real financial transactions.

## Future Improvements

- Automated unit and integration tests
- Rate limiting and account lockout controls
- Additional protection against cross-site request forgery (CSRF)
- Structured logging and monitoring
- Continuous integration with GitHub Actions
- Frontend integration with a customer and employee portal

## Author

**Limpho Sello**

GitHub: [Limpho-Sello](https://github.com/Limpho-Sello)
