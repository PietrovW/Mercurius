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




## Architecture
Clean Architecture to wzorzec architektury, który ma na celu budowanie aplikacji, które są łatwe do utrzymania, skalowania i testowania. Osiąga to poprzez podział aplikacji na różne warstwy o odrębnych odpowiedzialnościach:
Warstwa domeny (Domain Layer): Stanowi rdzeń aplikacji i zawiera zasady biznesowe oraz encje. Nie powinna mieć zależności zewnętrznych.
Warstwa aplikacji (Application Layer): Leży tuż za warstwą domeny i działa jako pośrednik między nią a innymi warstwami. Odpowiada za przypadki użycia aplikacji i eksponuje zasady biznesowe z warstwy domeny.
Warstwa infrastruktury (Infrastructure Layer): Tutaj implementujemy usługi zewnętrzne, takie jak bazy danych, przechowywanie plików, e-maile itp. Warstwa ta zawiera implementacje interfejsów zdefiniowanych w warstwie domeny.
Warstwa prezentacji (Presentation Layer): Odpowiada za interakcje użytkownika i dostarcza danych do interfejsu użytkownika.
Podstawową zasadą Clean Architecture jest to, że zależności powinny wskazywać z konkretnych warstw na abstrakcyjne warstwy wewnętrzne. Dzięki temu można w przyszłości zmieniać konkretne implementacje bez wpływu na inne obszary aplikacji. Dodatkowo, Clean Architecture stosuje strukturalne podejście do organizacji kodu, co ułatwia jego utrzymanie i testowanie1.
Warto również wspomnieć, że Clean Architecture i wzorzec Onion Architecture mają podobne cele i zasady, ale różnią się nieco w szczegółach. Onion Architecture skupia się na warstwie centralnej (zwanej Core), używając interfejsów i odwrócenia zależności, aby odseparować warstwy aplikacji i umożliwić wyższy stopień testowalności12.
Jeśli chcesz zgłębić temat Clean Architecture w C#, polecam zapoznać się z materiałami dostępnymi na stronie Code Maze. Tam znajdziesz więcej informacji oraz przykłady implementacji tego wzorca.

## Links 
* [Link architecture pattern](https://github.com/dotnet-architecture/eShopOnWeb)
* [Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)

## License
Mercurius is licensed under the [MIT LICENSE](https://choosealicense.com/licenses/mit/) 
