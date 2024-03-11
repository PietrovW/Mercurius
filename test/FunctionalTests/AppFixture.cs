using Alba;
using Api.Providers;
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
        Host = await AlbaHost.For<Program>();
    }
}
