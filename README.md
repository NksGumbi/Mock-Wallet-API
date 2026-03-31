# Mock Wallet API

A high-level mock wallet backend built with ASP.NET Core.

This repository contains a multi-project .NET solution that exposes wallet operations (credit, debit, refund, balance) through an HTTP API protected by an API key.

## What is in this repo

- `Wallet - API/Wallet.API`: API host (controllers, auth filter, startup/configuration, Swagger).
- `Wallet - API/Wallet.BL`: Business logic and data-access operations for wallet transactions.
- `Wallet - API/Wallet.DTO`: Shared request/response contracts, enums, and configuration objects.
- `Wallet - API/Wallet.API.sln`: Solution file for all projects.

## High-Level Architecture

- API layer receives requests and applies API key validation (`X-API-Key`).
- Business layer processes debit/credit/refund/balance workflows.
- DTO layer defines the models shared across layers.
- SQL Server is used for persistence (connection string is configured in appsettings).
- Serilog writes logs to console/file/SQL sink.

## Main Endpoints

Base route: `/api/Wallet`

- `POST /api/Wallet/GetBalance`
- `POST /api/Wallet/Debit`
- `POST /api/Wallet/Credit`
- `POST /api/Wallet/Refund`

## Run Locally

1. Ensure .NET SDK (8.0) and SQL Server are available.
2. Update settings in `Wallet - API/Wallet.API/appsettings.json`:
   - `ConnectionStrings:DBConnection`
   - `AppSettings:ApiKey`
   - `AppSettings:LogPath`
3. From repo root:

```bash
dotnet restore "Wallet - API/Wallet.API.sln"
dotnet run --project "Wallet - API/Wallet.API/Wallet.API.csproj"
```

4. Open Swagger at `http://localhost:<port>/swagger`.

## Notes

- This is a mock implementation intended for integration/testing scenarios.
- Keep secrets and real keys out of committed config files.
