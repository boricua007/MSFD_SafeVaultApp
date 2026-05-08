# Implementing Authentication and Authorization

## Part 2 Summary

This phase of SafeVault adds authentication and role-based authorization so that only verified users can sign in and only administrators can access protected administrative features.

---

## Deliverable 1: Authentication Implementation

### Login Flow
- **File**: [Controllers/AccountController.cs](Controllers/AccountController.cs)
- **Implementation**:
  - Exposes `GET /Account/Login` to render the sign-in form
  - Exposes `POST /Account/Login` to validate credentials
  - Rejects invalid login attempts with `Invalid username or password.`
  - Signs in successful users with cookie authentication
  - Redirects to a local `returnUrl` when present

### Login Model
- **File**: [Models/LoginModel.cs](Models/LoginModel.cs)
- **Implementation**:
  - Uses data annotations for required username and password input
  - Enforces minimum input requirements before authentication logic runs

### Password Hashing
- **File**: [Services/AuthService.cs](Services/AuthService.cs)
- **Implementation**:
  - Uses **BCrypt** to verify passwords securely
  - Rejects empty username/password values before verification
  - Compares the submitted plaintext password against the stored hash

### Seeded Accounts
- **File**: [Data/InMemoryDatabase.cs](Data/InMemoryDatabase.cs)
- **Implementation**:
  - Stores seeded demo users with BCrypt-hashed passwords
  - Includes a default administrator account and a standard user account
  - Assigns each user a role used later for authorization

### Authentication Middleware
- **File**: [Program.cs](Program.cs)
- **Implementation**:
  - Configures cookie authentication
  - Uses secure cookie settings:
    - `HttpOnly = true`
    - `SecurePolicy = Always`
    - `SameSite = Strict`
    - Sliding expiration enabled
  - Redirects unauthenticated users to `/Account/Login`

---

## Deliverable 2: Role-Based Authorization (RBAC)

### Role Assignment
- **File**: [Data/InMemoryDatabase.cs](Data/InMemoryDatabase.cs)
- **Implementation**:
  - `admin` is assigned the `Admin` role
  - `user1` is assigned the `User` role

### Role Claims on Sign-In
- **File**: [Controllers/AccountController.cs](Controllers/AccountController.cs)
- **Implementation**:
  - Adds `ClaimTypes.Name` for the signed-in user identity
  - Adds `ClaimTypes.Email` for user metadata
  - Adds `ClaimTypes.Role` so authorization can enforce role checks

### Protected Admin Route
- **File**: [Controllers/AdminController.cs](Controllers/AdminController.cs)
- **Implementation**:
  - Uses `[Authorize(Roles = "Admin")]`
  - Restricts the Admin Dashboard to administrator accounts only

### Protected Authenticated Route
- **File**: [Controllers/HomeController.cs](Controllers/HomeController.cs)
- **Implementation**:
  - Uses `[Authorize]` on the `/submit` action
  - Prevents unauthenticated form submissions

### UI Behavior Based on Authentication and Role
- **File**: [Views/Shared/_Layout.cshtml](Views/Shared/_Layout.cshtml)
- **Implementation**:
  - Shows `Login` when no user is authenticated
  - Shows `Hello, <username>` and `Logout` for signed-in users
  - Shows `Admin Dashboard` navigation only when the signed-in user is in the `Admin` role

### Access Denied Handling
- **File**: [Views/Account/AccessDenied.cshtml](Views/Account/AccessDenied.cshtml)
- **Implementation**:
  - Displays a dedicated access denied page for users who lack the required role

---

## Deliverable 3: Authentication and Authorization Testing

### Authentication Unit Tests
- **File**: [MSFD_SafeVault.Tests/AuthenticationTests.cs](../MSFD_SafeVault.Tests/AuthenticationTests.cs)
- **Coverage**:
  - Valid admin login succeeds
  - Valid standard user login succeeds
  - Invalid password is rejected
  - Unknown username is rejected
  - Empty credentials are rejected
  - Username matching is case-insensitive
  - Returned user records carry the correct role values

### Invalid Login Controller Test
- **File**: [MSFD_SafeVault.Tests/AccountControllerTests.cs](../MSFD_SafeVault.Tests/AccountControllerTests.cs)
- **Coverage**:
  - Invalid login attempt returns the login view
  - ModelState includes the expected error message

### Authorization Integration Tests
- **File**: [MSFD_SafeVault.Tests/AuthorizationIntegrationTests.cs](../MSFD_SafeVault.Tests/AuthorizationIntegrationTests.cs)
- **Coverage**:
  - Unauthenticated access to `/Admin` redirects to login
  - Authenticated non-admin users are denied access to `/Admin`
  - Authenticated admin users can access `/Admin` successfully

### Test Results
- **Status**: 21/21 tests passing
- **Command**:

```sh
dotnet test MSFD_SafeVault.Tests/MSFD_SafeVault.Tests.csproj
```

---

## Seeded Demo Credentials

- **Admin**: `admin` / `Admin@123`
- **User**: `user1` / `User@123`

---

## Outcome

SafeVault now includes:

- Secure user authentication using BCrypt password verification
- Cookie-based session management
- Role claims assigned at sign-in
- Route protection for both authenticated users and administrators
- Automated tests covering invalid login attempts, unauthorized access, and role-based access control

**Part 2 Status**: Complete