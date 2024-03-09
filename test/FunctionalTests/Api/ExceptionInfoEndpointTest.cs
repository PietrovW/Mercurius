using Application.ExceptionInfos.Commands.CreateExceptionInfoItem;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;

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
        await using var application = new WebApplicationFactory<Program>()
             .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
             {
                 services.AddDbContextFactory<MercuriusContext>(options
        => options.UseInMemoryDatabase($"MercuriusDatabase-{Guid.NewGuid().ToString()}"));
             }));
        try
        {
            using var client = application.CreateClient();

            // Act
            var result = await client.PostAsJsonAsync("/api/exceptionInfo", payload);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        catch (Exception ex)
        {

        }
        Assert.True(false);
    }
}

