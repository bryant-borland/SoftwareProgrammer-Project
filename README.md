# CRC Software Programmer Project

This project is part of the CRC Software Programmer interview process.
It demonstrates a simple ASP.NET Core Web API that manages product data using Entity Framework Core.

---
## Features
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core with InMemory database (easily swappable to SQL Server)
- Product model mapping to the provided schema
- Repository pattern with dependency injection
- Controller endpoint:
   - GET /api/products → returns a JSON array of all products
- Configuration: sample appsettings.json with placeholder connection string

## Bonus Enhancements
- Async/await support across the repository and controller
- Logging + error handling (with cancellation token support)
- Pagination support (/api/products?page=1&pageSize=10)
- Unit test project (SimpleProductAPI.Tests) with xUnit, Moq, EF Core InMemory, and FluentAssertions
- Repository tests: green
- Controller tests: most passing, two skipped with TODO notes for future improvements

## Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/bryant-borland/SoftwareProgrammer-Project.git
   cd SoftwareProgrammer-Project
   ```

2. Update the connection string in appsettings.json if needed.
3. Run the project
   ```bash
   dotnet run
   ```
  or press F5 in Visual Studio

## Assumptions
- Product entity includes three fields: ProductID, Name, and Price (matching the provided schema).
- No authentication, authorization, or advanced business rules were required.
- Database is assumed to be a local SQL Server instance (LocalDB).
- The focus of this project is core CRUD functionality, not extended features.

## .NET Version
.NET 8.0
