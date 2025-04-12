# Issue Manager API

**Issue Manager API** is a web application built using ASP.NET Core that allows authenticated users to manage issues (create, update, delete) across different version control platforms like GitHub, GitLab, and Bitbucket(optional).

## Features

- OAuth2 authentication via GitHub, GitLab, and Bitbucket(optional)
- Centralized issue management interface
- Authorization using JWT tokens
- Secure storage of access tokens in PostgreSQL
- Snake case JSON formatting
- Swagger UI for testing
- Daily logging using Serilog

## Technologies

- ASP.NET Core Web API
- PostgreSQL + EF Core
- Refit for typed HTTP clients
- AutoMapper for DTO mapping
- Serilog for logging
- Onion Architecture

## Installation

1. **Clone repository**

   ```
   git clone https://github.com/your-repo/issue-manager.git
   ```

2. **Configure secrets**  
   Use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or set environment variables for:

   - OAuth clients (GitHub, GitLab, Bitbucket)
   - JWT key
   - Encryption key
   - PostgreSQL connection string

3. **Run database migrations**

   ```
   dotnet ef database update
   ```

4. **Launch application**
   ```
   dotnet run
   ```

## Endpoints Overview

| Method | Endpoint             | Description                     |
| ------ | -------------------- | ------------------------------- |
| POST   | `/api/oauth/authUrl` | Get provider-specific OAuth URL |
| GET    | `/api/oauth/signin`  | Sign in and receive JWT         |
| POST   | `/api/issues/create` | Create new issue                |
| PUT    | `/api/issues/update` | Update existing issue           |
| DELETE | `/api/issues/delete` | Delete issue (if supported)     |

## Notes

- JWT must be included in all requests to `/api/issues` endpoints as a Bearer token.
- Swagger is available at `/swagger` (only in Development mode).
- Logging is stored daily in `Logs/log-yyyy-MM-dd.txt`.

## License

MIT
