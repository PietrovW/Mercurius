﻿using Api.Providers;
using Infrastructure.Data;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Wolverine;
using static Api.Extensions.AuthorizationConstants;

namespace Api.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, ConfigurationManager configuration)
    {
        var openIdConnectUrl = $"{configuration["Keycloak:auth-server-url"]}/realms/{configuration["Keycloak:realm"]}/.well-known/openid-configuration";

        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri(openIdConnectUrl),
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


    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {

     // services.AddKeycloakAuthentication(configuration);

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
       Policies.RequireRealmRole,
       builder => builder.RequireRealmRoles(Roles.RealmRole));

            options.AddPolicy(
                Policies.RequireClientRole,
                builder => builder.RequireResourceRoles(Roles.ClientRole));

        }).AddKeycloakAuthorization(configuration);

        return services;
    }
}
