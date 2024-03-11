using Application.ExceptionInfos.Commands.CreateExceptionInfoItem;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Oakton;
using System.Net;
using System.Net.Http.Json;
using Wolverine;

namespace FunctionalTests.Api;

public class ExceptionInfoEndpointTest
{
    [Fact]
    public async Task CreatingExceptionInfoItemCommandShouldReturnIt()
    {
        // Arrange
        var payload = new CreateExceptionInfoItemCommand()
        {
            InnerException = string.Empty,
            Message = string.Empty,
            Source = string.Empty,
            StackTrace = string.Empty,
            TargetSite = string.Empty
        };


        Environment.SetEnvironmentVariable("Provider", "InMemory");
        await using var application = new WebApplicationFactory<Program>()  

             .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
             {
                 services.DisableAllExternalWolverineTransports();
                 //
                // services.DisableAllExternalWolverineTransports();
                // x.ConfigureServices(services => services.DisableAllExternalWolverineTransports());
                 //Environment.SetEnvironmentVariable("ConnectionStrings__Postgres", "User ID=mercurius_user;Password=mercuriuscret;Server=localhost;Port=5432;Database=mercurius_db;");

                 //         services.AddDbContextFactory<MercuriusContext>(options
                 //=> options.UseInMemoryDatabase($"MercuriusDatabase-{Guid.NewGuid().ToString()}"));
             })
             
             
             );

        try
        {
            using var client = application.CreateClient();

            // Act
            // var result = await client.PostAsync("/api/exceptionInfo", payload);
            var result = await client.GetAsync("/api/exceptionInfo");
            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        catch (Exception ex)
        {

        }
        Assert.True(false);
    }

    public class AppFixture : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            OaktonEnvironment.AutoStartHost = true;

            Host = await AlbaHost.For<Program>(x =>
            {
                // I'm overriding 
                x.ConfigureServices(services =>
                {
                    // Let's just take any pesky message brokers out of
                    // our integration tests for now so we can work in
                    // isolation
                    services.DisableAllExternalWolverineTransports();

                    // Just putting in some baseline data for our database
                    // There's usually *some* sort of reference data in 
                    // enterprise-y systems
                    services.InitializeMartenWith<InitialAccountData>();
                });
            });
        }

        public IAlbaHost Host { get; private set; }

        public Task DisposeAsync()
        {
            return Host.DisposeAsync().AsTask();
        }
    }
}

