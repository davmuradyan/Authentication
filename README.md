# 🔐 ASP.NET Core Authentication Starter

A reusable, invitation-based authentication system built with **Clean Architecture** and **ASP.NET Core**. Designed to be cloned and adapted to any project that needs multi-role auth with JWT, without rewriting the same boilerplate every time.

---

## ✨ Features

- **Invitation-based onboarding** — no self sign-up; accounts are created by invitation only
- **Role-based access control** with fine-grained policies (e.g. `Company.Create`)
- **Global admin** account pre-seeded and ready to go
- **JWT authentication** with refresh token support
- **Logout from all devices** via refresh token invalidation
- **Email activation** — invited users set their password via a secure activation link
- **SQL Server + EF Core** — migrations pre-configured and ready to run
- **Clean Architecture** — easy to extend without touching what you don't need

---

## 🏗️ Architecture Overview

```
├── Endpoints        → Controllers, Program.cs (ASP.NET Core Web API)
├── Application      → Business logic, services, repository interfaces
├── Infrastructure   → EF Core, database config, seed data, repository implementations
├── Domain           → Entities (Factory pattern)
└── Contracts        → DTOs and result objects
```

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/davmuradyan/Authentication.git
cd Authentication
```

### 2. Configure your settings

Open `appsettings.Development.json` and fill in your values:

```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-min-32-chars",
    "Issuer": "YourAppName",
    "Audience": "YourAppName",
    "ExpiryMinutes": 15
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "Your Name",
    "AppPassword": "your-app-password"
  }
}
```

> **Gmail tip:** Use an [App Password](https://myaccount.google.com/apppasswords) rather than your actual account password. Two-factor authentication must be enabled on your Google account for this to work.

> **JWT tip:** Your `SecretKey` should be a long random string (32+ characters). You can generate one [here](https://generate-secret.vercel.app/64).

### 3. Set up the global admin seed data

Open `Infrastructure/Database/Seeds/InitialSeed.cs` and edit the following block:

```sql
INSERT INTO Users (Id, Email, PasswordHash, IsEmailConfirmed, IsActive, CreatedAt, UpdatedAt)
VALUES (
    'f9e8d7c6-b5a4-3210-fedc-ba9876543210',
    'your-email@example.com',          -- ← your admin email
    'your-bcrypt-hashed-password',     -- ← see note below
    1, 1, '2026-01-01', '2026-01-01'
);

INSERT INTO UserRoles (UserId, RoleId, AssignedAt)
VALUES (
    'f9e8d7c6-b5a4-3210-fedc-ba9876543210',
    'a1b2c3d4-e5f6-7890-abcd-ef1234567890',
    '2026-01-01'
);

INSERT INTO CompanyUsers (Id, UserId, Name, Surname, MiddleName)
VALUES (
    '12345678-abcd-ef90-1234-567890abcdef',
    'f9e8d7c6-b5a4-3210-fedc-ba9876543210',
    'Name',          -- ← your first name
    'Surname',       -- ← your last name
    'Middlename'     -- ← your middle name (or remove if unused)
);
```

> **Password hashing:** The project uses BCrypt. To generate your hash, use an online tool like [bcrypt-generator.com](https://bcrypt-generator.com/) with **12 rounds**, then paste the result into `PasswordHash`.

You can also change the UUIDs to anything you prefer — just make sure they're consistent across all three `INSERT` statements.

### 4. Run migrations and start the app

```bash
dotnet ef database update
dotnet run --project Endpoints
```

The database will be created and seeded automatically on first run. Navigate to `/swagger` to explore the API.

---

## 🔑 How Authentication Works

1. The **global admin** account is created via seed data and holds all policies
2. To add a user, global admin (or anyone with the right policy) sends an **invitation** to their email
3. The invited user receives an activation link, sets their password, and the account becomes active
4. All subsequent auth is handled via **JWT access tokens** + **refresh tokens**
5. Policies (e.g. `Company.Create`) are attached to roles and enforced on controllers with `[Authorize(Policy = "Company.Create")]`

---

## 🛠️ Adapting to Your Project

This starter is **intentionally incomplete** — it gives you the foundation, not the full application. Here's what you're expected to add or modify:

| What | Where | Notes |
|---|---|---|
| New entities | `Domain` | Use the existing Factory pattern |
| New policies | `Infrastructure` + controllers | Register in policy config, apply with `[Authorize]` |
| Role/permission management endpoints | `Endpoints` + `Application` | Only seed data handles this by default |
| Hashing algorithm | One method in `Infrastructure` | Swap SHA256 for anything else in a single place |
| Email templates | `Infrastructure` | Edit the invitation/activation email content |

---

## 📦 Tech Stack

- **ASP.NET Core** — Web API
- **Entity Framework Core** — ORM
- **SQL Server** — Database
- **JWT Bearer** — Authentication
- **BCrypt** — Password hashing
- **SMTP** — Email delivery

---

## ⚠️ Important Notes

- Never commit `appsettings.Development.json` with real credentials to a public repository. Add it to `.gitignore`.
- The `global-admin` role and its policies are created in seed data. All other role/permission management is left for you to implement based on your needs.
- Refresh tokens are stored in the database — `logout from all devices` invalidates all of them at once.
