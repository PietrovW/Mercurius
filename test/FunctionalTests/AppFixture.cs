using Alba;
using Api.Providers;
using FunctionalTests.AuthHandlerTest;
using Infrastructure.Data;
using Microsoft.AspNetCore.TestHost;
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
            x.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                })
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
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
