# CRC Software Programmer Project

This project is part of the CRC Software Programmer interview process.  
It demonstrates a simple ASP.NET Core Web API connected to a SQL Server database.

---
## Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/bryant-borland/SoftwareProgrammer-Project.git
   cd SoftwareProgrammer-Project

2. Update the connection string in appsettings.json if needed.
3. Run the project
   ```bash
   dotnet run
  or use F5 in Visual Studio

## Assumptions
1. Product entity includes three fields: ProductID, Name, and Price.
2. No authentication, authorization, or advanced business rules were required.
3. Database is assumed to be a local SQL Server instance (LocalDB).
4. Focus of this project is core CRUD functionality, not extended features.

## .NET Version
.NET 8.0
