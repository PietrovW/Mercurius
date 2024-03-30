using Alba;
using Api.Authorization.Decision;
using Api.Providers;
using FluentAssertions.Common;
using FunctionalTests.AuthHandlerTest;
using Infrastructure.Data;
using JasperFx.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Oakton;

namespace FunctionalTests;

public class AppFixture : IDisposable, IAsyncLifetime
{
    public IAlbaHost Host { get; private set; }

    public Task DisposeAsync()
    {
        return Host.StopAsync();
    }

    public void Dispose()
    {
        Host?.Dispose();
    }

    public async Task InitializeAsync()
    {
        OaktonEnvironment.AutoStartHost = true;
        Environment.SetEnvironmentVariable("Provider", Provider.InMemory.Name);
        Host = await AlbaHost.For<Program>(x =>
        {
            x.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IAuthorizationHandler, TestAuthHandler>();
                services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = "teste");
               // services.AddAuthentication()
                 //     .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
            });
        });
        
        using (var scope = Host.Services.CreateScope())
        {
            var serviceScope = scope.ServiceProvider;
          
            var context = serviceScope.GetService<MercuriusContext>();
            
            DataSeeder.SeedCountries(context:context!);
        }
    }

    
}
