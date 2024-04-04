
## Konfiguracja Keycloak


Keycloak configuration: 

Testing 

```
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = $"{authServerUrl}realms/{realms}";
    options.MetadataAddress = $"{authServerUrl}realms/{realms}/.well-known/openid-configuration";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidIssuer = $"{authServerUrl}realms/{realms}",
        ValidateLifetime = true
    };
    options.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();
            c.Response.StatusCode = 401;
            c.Response.ContentType = "text/plain";
            return c.Response.WriteAsync(c.Exception.ToString());
        }
    };
});
```



# Konfiguracja Docker Compose

W tym kroku skonfigurujemy plik docker-compose.yaml, aby skonfigurować Keycloak za pomocą Docker Compose. Ta konfiguracja definiuje dwie usługi: postgres_db i keycloak, pgadmin wraz z powiązanymi woluminami.
```
version: '3.9'

services:
  postgres_db:
     image: postgres:${POSTGRES_VERSION}
     restart: unless-stopped
     ports:
      - 5432:5432
     volumes:
      - volumes_db:/var/lib/postgresql/data
     environment:
       POSTGRES_DB: ${POSTGRESQL_DB}
       POSTGRES_USER: ${POSTGRES_USER:-keycloak}
       POSTGRES_PASSWORD: ${POSTGRES_PASSWORD:-eX4mP13p455w0Rd}
     networks:
      - backend

  pgadmin:
    container_name: pgadmin
    image: dpage/pgadmin4:${PGADMIN_VERSION}
    environment:
      PGADMIN_DEFAULT_EMAIL: user@domain.local
      PGADMIN_DEFAULT_PASSWORD: ${KEYCLOAK_ADMIN:-admin}
      POSTGRES_USER: ${POSTGRES_USER:-keycloak}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD:-eX4mP13p455w0Rd}
      PGADMIN_CONFIG_SERVER_MODE: 'False'
    restart: always  
    ports:
      - 5050:80
    depends_on:
      - postgres_db
    networks:
      - backend
    volumes:
      - ./servers.json:/pgadmin4/servers.json

  keycloak:
    image: quay.io/keycloak/keycloak:${KEYCLOAK_VERSION}
    environment:
      KC_DB: postgres
      KC_DB_USERNAME: ${POSTGRES_USER:-keycloak}
      KC_DB_PASSWORD: ${POSTGRES_PASSWORD:-eX4mP13p455w0Rd}
      KC_DB_URL: jdbc:postgresql://postgres_db/keycloak
      DB_DATABASE: ${POSTGRESQL_DB}
      KEYCLOAK_ADMIN: ${KEYCLOAK_ADMIN:-admin}
      KEYCLOAK_ADMIN_PASSWORD: ${KEYCLOAK_ADMIN_PASSWORD:-admin}
    ports:
      - 8080:8080
      - 8443:8443
    depends_on:
      - postgres_db
    command:
      - start-dev
    networks:
      - backend

networks:
  backend:
    name: backend
    driver: bridge 

volumes:
  volumes_db:
    driver: local 
```

kowiguracaja pliku  .env

```
KEYCLOAK_VERSION=23.0.6
POSTGRES_VERSION=16
PGADMIN_VERSION=5.1
DB_VENDOR=POSTGRES
POSTGRES_USER=keycloak
POSTGRES_PASSWORD=eX4mP13p455w0Rd
KEYCLOAK_ADMIN_PASSWORD=admin
KC_DB_URL_PORT=5432
POSTGRESQL_URL=0.0.0.0
POSTGRESQL_DB=keycloak
KC_DB_USERNAME=mercurius_user
KC_DB_PASSWORD=mercuriuscret
KC_DB_SCHEMA=public
KEYCLOAK_ADMIN=admin
```


Aby uruchomić usługe Keycloak, uruchom następującego polecenie w terminalu:

```
docker compose up -d
```

Zainicjowanie Keycloak może zająć trochę czasu potrwać  przy pierwszym uruchomieniu. Po uruchomieniu możesz uzyskać dostęp do  Keycloak pod adresem
```
http://localhost:8080
https://localhost:8443
```

strona startowa 
![keycloak_start](/images/keycloak_start.png)

strona logowania do Keycloak
Zaloguj się do konsoli administracyjnej Keycloak, kliknij Dodaj dziedzinę z menu rozwijanego w lewym panelu.
![keycloak_home_login.png](/images/keycloak_home_login.png)

Ta opcja menu przeniesie Cię na stronę Dodaj Realm. Podaj nazwę dziedziny, którą chcesz zdefiniować i kliknij przycisk Utwórz. Alternatywnie możesz zaimportować dokument JSON, który definiuje Twoją nową Realm. Omówimy to bardziej szczegółowo w rozdziale Eksport i import.
![create_realn_mercurius.png](/images/create_realn_mercurius.png)
![keycloak_home_mercurius.png](/images/keycloak_home_mercurius.png)

