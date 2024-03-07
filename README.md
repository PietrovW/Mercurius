![Image Alt text](/images/logo.png)

# Readme Mercurius
Aplikacja do zbierania błędów programów to narzędzie, które umożliwia wykrywanie, 
monitorowanie i raportowanie błędów w oprogramowaniu



## Getting started


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

Tworzenie migracji :

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
## Application architecture



## Links

[Link architecture pattern](https://github.com/dotnet-architecture/eShopOnWeb)
[Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)

