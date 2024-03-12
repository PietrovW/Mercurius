using Alba;
using Api.Providers;
using Infrastructure.Data;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oakton;

namespace FunctionalTests;

public class AppFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; private set; }

    public async Task DisposeAsync()
    {
       await Host.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        OaktonEnvironment.AutoStartHost = true;
        Environment.SetEnvironmentVariable("Provider", Provider.InMemory.Name);
        Host = await AlbaHost.For<Program>() ;

        using (var scope = Host.Services.CreateScope())
        {
            var serviceScope = scope.ServiceProvider;
            var context = serviceScope.GetService<MercuriusContext>();
            DataSeeder.SeedCountries(context:context!);
        }
    }
}
