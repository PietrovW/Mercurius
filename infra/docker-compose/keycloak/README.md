
## Keycloak


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
