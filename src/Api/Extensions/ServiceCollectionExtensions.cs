using Api.Authentication;
using Api.Authorization.Decision;
using Api.Providers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
namespace Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri($"{configuration["Keycloak:auth-server-url"]}realms/{configuration["Keycloak:realm"]}/.well-known/openid-configuration"),
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
            });
        });
        return services;
    }

    public static IServiceCollection AddConfigurationDataBase(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContextFactory<MercuriusContext>(options =>
        {
            var provider = configuration.GetValue("provider", Provider.Postgres.Name);
            if (provider == Provider.Postgres.Name)
            {
                options.UseNpgsql(
                    connectionString: configuration.GetConnectionString(Provider.Postgres.Name)!,
                   npgsqlOptionsAction: x => x.MigrationsAssembly(Provider.Postgres.Assembly));
            }
            else if (provider == Provider.MySql.Name)
            {
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
                options.UseMySql(connectionString: configuration.GetConnectionString(Provider.MySql.Name)!, serverVersion: serverVersion,
                                         mySqlOptionsAction: x => x.MigrationsAssembly(Provider.MySql.Assembly));
            }
            else if (provider == Provider.SqlServer.Name)
            {
                options.UseSqlServer(connectionString: configuration.GetConnectionString(Provider.SqlServer.Name)!,
                                  sqlServerOptionsAction: x => x.MigrationsAssembly(Provider.SqlServer.Assembly));
            }
            else if (provider == Provider.InMemory.Name)
            {
                options.UseInMemoryDatabase($"MercuriusDatabase-{Guid.NewGuid()}");
            }
            else
            {
                throw new Exception($"Unsupported provider: {provider}");
            }
        });
        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        string authServerUrl= configuration["Keycloak:AuthServerUrl"]!;
        string realms = configuration["Keycloak:realm"]!;
        services.AddTransient<IClaimsTransformation>(_ =>
           new RolesClaimsTransformation("role", authServerUrl));
        services.AddHttpContextAccessor();
        services.AddSingleton<IAuthorizationHandler, DecisionRequirementHandler>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = $"{authServerUrl}realms/{realms}";
            options.MetadataAddress = $"{authServerUrl}realms/{realms}/.well-known/openid-configuration";
            options.RequireHttpsMetadata = false; //dev
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

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("email_verified", "true")
                .Build();


            options.AddPolicy("rola_add"
                   , builder => builder.AddRequirements(new DecisionRequirement("rola_add"))
               );
        });

        services.AddAuthorization();
        
        return services;
    }

    public static IServiceCollection AddApplicationSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        string authServerUrl = configuration["Keycloak:AuthServerUrl"]!;
        string realms = configuration["Keycloak:realm"]!;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Auth",
                Type = SecuritySchemeType.OAuth2,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                },
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{authServerUrl}realms/{realms}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{authServerUrl}realms/{realms}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>(),
                    }
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
            });
        });
        return services;
    }

    public static IApplicationBuilder UseApplicationSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(s => s.OAuthClientId("test-client"));
        return app;
    }
}
