[![100 - COMMITOW](https://img.shields.io/badge/100-COMMITOW-2ea44f)](https://100commitow.pl/)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)


![Logo](/images/logo.png)

![swagger](/images/swagger.png)

# Readme Mercurius
A program error collection application is a tool that allows you to detect,
monitoring and reporting software errors


Launch Apps, using the command
```
    docker-compose up
```
## Features

- Bug reporting: Users can report bugs, defects and issues related to the application.
- Assignment and Tracking: Bugs are assigned to the appropriate team members and then tracked through the remediation process.
- Priorities and statuses: Each bug is assigned a priority (e.g. critical, high, low) and a status (e.g. open, under repair, closed).
- Change History: The app records a history of changes to bugs, making it easy to track your progress.
- Notifications: Automatic notifications of changes in errors (e.g. assignment, status change) are sent to the appropriate users.
- Statistics and reports: Generate reports on the number of bugs, repair times and other statistics.
- Saving errors reported by applications.
- Automatic error grouping: can group similar errors together, making them easier to manage and track
- SDK will allow you to collect information about errors and send them to API (C# language)

Creating a migration:

```
 dotnet ef migrations add InitialCreate --project ../Migrations/Infrastructure.MySql  -- --provider MySql
```

```
 dotnet ef migrations add InitialCreate --project ../Migrations/Infrastructure.Postgres  -- --provider Postgres
```

```
 dotnet ef migrations add InitialCreate --project ../Migrations/Infrastructure.SqlServer  -- --provider SqlServer
```

## Technologies
* [ASP.NET Core 8](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [Entity Framework Core 8](https://docs.microsoft.com/en-us/ef/core/)
* [wolverine](https://wolverine.netlify.app/)
* [SqlServer](Microsoft.EntityFrameworkCore.SqlServer)
* [PostgreSQL](Npgsql.EntityFrameworkCore.PostgreSQL)
* [MySql](Pomelo.EntityFrameworkCore.MySql)
* [Docker Compose](https://docs.docker.com/compose/)
* [xunit](https://xunit.net/)
## Application architecture


## Links

[Link architecture pattern](https://github.com/dotnet-architecture/eShopOnWeb)
[Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)


## License

Mercurius is licensed under the [MIT LICENSE](https://choosealicense.com/licenses/mit/) 
