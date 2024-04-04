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
using System.Text;
namespace Api.Extensions;

internal static class ServiceCollectionExtensions
{

    public static IServiceCollection AddAndConfigLocalization(this IServiceCollection services)
    {
        //services.AddLocalization(options => options.ResourcesPath = "Resources");

        //var supportedCultures = new List<CultureInfo> { new("en"), new("fa") };
        //services.Configure<RequestLocalizationOptions>(options =>
        //{
        //    options.DefaultRequestCulture = new RequestCulture("fa");
        //    options.SupportedCultures = supportedCultures;
        //    options.SupportedUICultures = supportedCultures;
        //});

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
        string issuerSigningKey = configuration["JwtBearer:IssuerSigningKey"]!;
        services.AddTransient<IClaimsTransformation>(_ =>
           new RolesClaimsTransformation("role"));
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
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = $"{authServerUrl}realms/{realms}",
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
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

            options.AddPolicy(RolesConstants.RolaAdd
                   , builder => builder.AddRequirements(new DecisionRequirement(RolesConstants.RolaAdd))
               );
        });
        
        return services;
    }

    public static IServiceCollection AddApplicationSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        string authServerUrl = configuration["Keycloak:AuthServerUrl"]!;
        string realms = configuration["Keycloak:realm"]!;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
            options.OperationFilter<SwaggerLanguageHeader>();
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
            options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
            });

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Mercurius API",
                Version = "v1",
                Description = "Mercurius API",
               
            });
        });
        return services;
    }
    const string SwaggerRoutePrefix = "api-docs";
    public static IApplicationBuilder UseApplicationSwagger(this WebApplication app, IConfiguration configuration)
    {
        string resource = configuration["Keycloak:resource"]!;
        app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/docs.json"; });
        app.UseSwaggerUI(options =>
        {
            options.OAuthClientId(resource);
            options.RoutePrefix = SwaggerRoutePrefix;
            foreach (var description in app.DescribeApiVersions())
                options.SwaggerEndpoint($"/{SwaggerRoutePrefix}/{description.GroupName}/docs.json", description.GroupName.ToUpperInvariant());
        });
        app.MapGet("/", () => Results.Redirect($"/swagger")).ExcludeFromDescription();
        return app;
    }
}
