using Alba;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Oakton;

namespace FunctionalTests;

public class AppFixture : IAsyncLifetime
{
    public IAlbaHost Host { get; private set; }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public async Task InitializeAsync()
    {
        OaktonEnvironment.AutoStartHost = true;

        Environment.SetEnvironmentVariable("Provider", "InMemory");
        Host = await AlbaHost.For<Program>(x => {
            x.ConfigureServices((context, services) =>
            {
                services.AddDbContextFactory<MercuriusContext>(options
                 => options.UseInMemoryDatabase($"MercuriusDatabase-{Guid.NewGuid()}"));
            });
        });
    }
}
