[![100 - COMMITOW](https://img.shields.io/badge/100-COMMITOW-2ea44f)](https://100commitow.pl/)
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)


![Logo](/images/logo.png)


# Readme Mercurius
A program error collection application is a tool that allows you to detect,
monitoring and reporting software errors


Launch Apps, using the command
```
    docker-compose up
```
## Features
- Zgłaszanie błędów: Użytkownicy mogą zgłaszać błędy, defekty i problemy związane z aplikacją.
- Przypisywanie i śledzenie: Błędy są przypisywane do odpowiednich członków zespołu, a następnie śledzone w procesie naprawy.
- Priorytety i statusy: Każdy błąd ma przypisany priorytet (np. krytyczny, wysoki, niski) oraz status (np. otwarty, w trakcie naprawy, zamknięty).
- Historia zmian: Aplikacja rejestruje historię zmian w błędach, co ułatwia śledzenie postępów.
- Powiadomienia: Automatyczne powiadomienia o zmianach w błędach (np. przypisanie, zmiana statusu) są wysyłane do odpowiednich użytkowników.
- Statystyki i raporty: Generowanie raportów na temat ilości błędów, czasu naprawy i innych statystyk.
- Zapisywanie błędów zgloszonych przez aplikacje.
- Automatyczne grupowanie błędów: potrafi grupować podobne błędy, co ułatwia ich zarządzanie i śledzenie
- SDK pozwoli na zbieranie informacji o błędach i ich przesyłanie do Api ( jezyk C#)

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


