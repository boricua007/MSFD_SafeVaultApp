# MSFD SafeVaultApp

## Overview

This ASP.NET Core MVC web application demonstrates secure user input handling, authentication, and authorization. The project is designed as a lab exercise to show how common web threats such as XSS, SQL injection, and unauthorized access can be mitigated with defensive coding patterns.

## Features

✅ ASP.NET Core MVC form workflow for collecting user input  
✅ Model validation using data annotations  
✅ Input sanitization for script tags, HTML tags, and SQL control tokens  
✅ Secure parameterized SQL query example  
✅ Cookie-based authentication with secure settings  
✅ Role-based authorization (Admin/User) for protected routes  
✅ BCrypt password hashing and verification  
✅ NUnit test coverage for sanitization, authentication, and authorization scenarios  
✅ Automatic TRX output plus readable Markdown test summary

## Getting Started

1. Clone the repository or download the source code.
2. Open the solution in Visual Studio or your preferred C# IDE.
3. Restore packages (if prompted).
4. Run the web app:

   ```sh
   dotnet run --project MSFD_SafeVault/MSFD_SafeVault.csproj
   ```

5. Open the URL shown in the terminal (typically http://localhost:5222).
6. Sign in using seeded demo credentials:

  - `admin` / `Admin@123` (Admin role)
  - `user1` / `User@123` (User role)

7. Submit values through the form to observe validation and sanitization flow.
8. Verify role-based access:

  - Admin can access `/Admin`
  - Regular users are denied admin route access

## Project Structure

```text
MSFD_SafeVault/
│
├─ Controllers/
│  └─ HomeController.cs            # MVC actions, submit endpoint, sanitization flow
│  ├─ AccountController.cs         # Login, logout, access denied actions
│  └─ AdminController.cs           # Admin-only dashboard ([Authorize(Roles = "Admin")])
├─ Data/
│  ├─ InMemoryDatabase.cs          # In-memory users + roles + BCrypt hashed passwords
│  └─ SecureDatabaseExample.cs     # Parameterized SQL example (safe query pattern)
├─ Models/
│  ├─ UserInputModel.cs            # Form input model with validation attributes
│  ├─ LoginModel.cs                # Login model with validation attributes
│  └─ ErrorViewModel.cs
├─ Services/
│  └─ InputSanitizer.cs            # Input sanitization logic
│  └─ AuthService.cs               # BCrypt credential verification
├─ Views/
│  ├─ Home/Index.cshtml            # User form UI
│  ├─ Account/Login.cshtml         # Login UI
│  ├─ Account/AccessDenied.cshtml  # Access denied UI
│  └─ Admin/Index.cshtml           # Admin dashboard UI
├─ Program.cs                      # ASP.NET Core startup, auth middleware, routing
└─ MSFD_SafeVault.csproj

MSFD_SafeVault.Tests/
│
├─ InputSanitizerTests.cs          # Sanitizer behavior tests
├─ SqlInjectionTests.cs            # SQL-injection pattern tests
├─ AuthenticationTests.cs          # Auth service credential and role tests
├─ AccountControllerTests.cs       # Invalid login controller test
├─ AuthorizationIntegrationTests.cs # Route protection tests by auth state/role
├─ scripts/Format-TestResults.ps1  # Generates readable test summary from latest TRX
└─ TestResults/
   ├─ *.trx                        # Raw test result output
   └─ test-results-summary.md      # Readable test report (auto-generated)
```

## Key Concepts Demonstrated

- Input validation: Model attributes enforce required fields and valid email format.
- Input sanitization: Script blocks, HTML tags, and risky SQL tokens are stripped.
- SQL injection defense: Parameterized query example uses @username parameter.
- Authentication: Cookie auth issues secure session cookies after login.
- Password security: BCrypt verifies hashed passwords.
- Authorization (RBAC): Role claims enforce admin-only route access.
- Separation of concerns: Controller, model, service, and data logic are separated.
- Test reporting automation: Each run writes machine-readable and human-readable results.

## Usage

- Log in from `/Account/Login` with one of the seeded accounts.
- Open the home page form and submit Username and Email.
- The app validates and sanitizes input before storing it in memory.
- The controller includes a secure SQL lookup example using parameterized commands.
- Visit `/Admin` to verify role-based route protection.

## Running Tests

Run all tests:

```sh
dotnet test MSFD_SafeVault.Tests/MSFD_SafeVault.Tests.csproj
```

After each run:

- TRX results are written under MSFD_SafeVault.Tests/TestResults.
- A readable summary is written to:
  MSFD_SafeVault.Tests/TestResults/test-results-summary.md

## Security Notes

- This is a demo app. For production, replace in-memory users with a persistent
  identity store (for example, ASP.NET Identity + EF Core).
- Use output encoding and Content Security Policy (CSP) in addition to sanitization.
- Store secrets and encryption keys securely (for example, Azure Key Vault).
- Prefer persistent storage with strict least-privilege database credentials.

## About

.NET 9 MVC web application built for the Microsoft Security and Authentication course as part of the Microsoft Full-Stack Developer Certification track. This project demonstrates practical secure input handling and test-driven verification of security-focused logic.

## Author

Daisy Viruet-Allen (boricua007)  
GitHub: https://github.com/boricua007
