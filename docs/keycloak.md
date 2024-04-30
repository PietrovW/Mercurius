## Keycloak
Keycloak to otwarte oprogramowanie umożliwiające jednokrotne logowanie (SSO) oraz zarządzanie tożsamością. Jest to rozwiązanie, które dodaje uwierzytelnianie i autoryzację do aplikacji i usług z minimalnym wysiłkiem. Oto kilka kluczowych cech Keycloak:

Jednokrotne logowanie (SSO): Użytkownicy uwierzytelniają się w Keycloak, a nie w poszczególnych aplikacjach. To oznacza, że Twoje aplikacje nie muszą obsługiwać formularzy logowania, uwierzytelniania użytkowników ani przechowywania ich danych. Po zalogowaniu się w Keycloak, użytkownicy nie muszą ponownie logować się, aby uzyskać dostęp do innej aplikacji.
To samo dotyczy wylogowywania się.
Broker tożsamości i logowanie społecznościowe: Dodawanie logowania za pomocą sieci społecznościowych jest proste. Wystarczy wybrać społeczność, którą chcesz dodać, bez konieczności zmian w kodzie aplikacji. Keycloak może również uwierzytelniać użytkowników za pomocą istniejących dostawców tożsamości OpenID Connect lub SAML 2.0.
Federacja użytkowników: Keycloak ma wbudowane wsparcie dla połączenia z istniejącymi serwerami LDAP lub Active Directory.
Możesz również zaimplementować własny dostawca, jeśli masz użytkowników w innych źródłach, takich jak baza danych relacyjna.

Konsola administracyjna: Administratorzy mogą centralnie zarządzać wszystkimi aspektami serwera Keycloak za pomocą konsoli administracyjnej. Mogą włączać i wyłączać różne funkcje, konfigurować federację tożsamości i zarządzać aplikacjami, usługami oraz politykami autoryzacji.
Standardowe protokoły: Keycloak opiera się na standardowych protokołach i obsługuje OpenID Connect, OAuth 2.0 i SAML.
Jeśli potrzebujesz bardziej szczegółowych informacji, możesz sprawdzić dokumentację Keycloak1. To narzędzie jest szczególnie przydatne dla zabezpieczania aplikacji i usług w kontekście tożsamości i autoryzacji.


## Source
[Mapping, customizing, and transforming claims in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/claims?view=aspnetcore-8.0)
[Mapping, customizing, and transforming claims in ASP.NET Core GitHub](https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/security/authentication/claims.md)
 
![Home](/images/Home.png)
![login](/images/login.png)
![create_realm](/images/create_realm.png)
![create_realm_mercurius](/images/create_realm_mercurius.png)
![Swagger Login](/images/swagger_Login.png)
