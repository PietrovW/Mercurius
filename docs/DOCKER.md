## Docker

W dzisiejszym nowoczesnym środowisku rozwoju oprogramowania budowanie skalowalnych i skonteneryzowanych aplikacji jest kluczowe. W tym wpisie na zadbam, jak zbudować interfejs API przy użyciu popularnego frameworka .NET Core, umieścić go w kontenerze Docker i wdrożyć. Ta kombinacja oferuje potężne i elastyczne środowisko do tworzenia, pakowania i zarządzania Twoim interfejsem API. 

Przed przystąpieniem do kroków upewnij się, że masz zainstalowane następujące narzędzia: 
* .NET Core SDK 
* Docker 

Krok 1: Tworzenie interfejsu API przy użyciu .NET Core 

 Otwórz swoje ulubione środowisko programistyczne (IDE) lub wiersz poleceń (CLI). 
 
 Utwórz nowy projekt .NET Core Web API, wykonując poniższą komendę: 
 
 Struktura plików powinna wyglądać podobnie do tej: 
 
 Zawiera pliki Program kontrolerów odpowiedzialnych za obsługę żądań API i definiowanie punktów końcowych. 
 
 Plik Mercurius.csproj to plik projektu, który zarządza zależnościami i konfiguracją projektu. 
 
 Plik Program.cs to punkt wejścia do aplikacji i konfiguruje aplikację i usługi. 
 
 Teraz możemy zbudować i uruchomić nasz projekt interfejsu API: 

Otwórz przeglądarkę i odwiedź adres URL: 4(http://localhost:5248/api/mercurius) 

Krok 2: Umieszczanie interfejsu API w kontenerze Docker 

 W katalogu głównym projektu API utwórz plik o nazwie Dockerfile. 
 
 Otwórz plik Dockerfile i dodaj następujący kod: 
 
 Upewnij się, że w pliku Dockerfile używasz takiej samej wersji SDK .NET, jak w Twoim projekcie. W moim przypadku używam .NET 8, więc sprawdź plik .csproj. 
 
 Dockerfile krok po kroku: 
 
 Instrukcja FROM ustawia obraz bazowy jako obraz SDK .NET, który umożliwia nam budowanie i publikowanie API. 
 
 Instrukcja WORKDIR ustawia katalog roboczy wewnątrz kontenera na /app. 
 
 Polecenie COPY kopiuje plik .csproj do katalogu /app w kontenerze. 
 
 Polecenie RUN dotnet restore przywraca paczki NuGet dla projektu. 
 
 Drugie polecenie COPY kopiuje pozostałe pliki projektu do katalogu /app w kontenerze. 
 
 To wszystko! Pomyśln 

```
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Migrations/Infrastructure.MySql/Infrastructure.MySql.csproj", "src/Migrations/Infrastructure.MySql/"]
COPY ["src/Migrations/Infrastructure.Postgres/Infrastructure.Postgres.csproj", "src/Migrations/Infrastructure.Postgres/"]
COPY ["src/Migrations/Infrastructure.SqlServer/Infrastructure.SqlServer.csproj", "src/Migrations/Infrastructure.SqlServer/"]
RUN dotnet restore "./src/Api/./Api.csproj"
COPY . .
WORKDIR "/src/src/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]

```

## Źródła 
1. [Building an API with .NET Core, Docker and Kubernetes](https://medium.com/@josesousa8/building-an-api-with-net-core-docker-and-kubernetes-aa3e02add0c) 
2. [Containerization and Kubernetes: Empowering Modern Application Development](https://www.wwt.com/blog/containerization-and-kubernetes-empowering-modern-application-development) 
3. [Containerization and Microservices: The Future of Building and ...](https://dev.to/kingsley/containerization-and-microservices-the-future-of-building-and-deploying-modern-applications-1pia) 
4. [Containers | Kubernetes](https://kubernetes.io/docs/concepts/containers/) 
5. [Containerization and Kubernetes - Scaler Topics](https://www.scaler.com/topics/kubernetes/kubernetes-containerization/) 
6. [What is Kubernetes Containerization? | Glossary | HPE](https://www.hpe.com/us/en/what-is/kubernetes-containerization.html) 
7. [Demystifying containers, Docker, and Kubernetes](https://cloudblogs.microsoft.com/opensource/2019/07/15/how-to-get-started-containers-docker-kubernetes/)
8. [minikube start | minikube (k8s.io)](https://minikube.sigs.k8s.io/docs/start/) 

 



