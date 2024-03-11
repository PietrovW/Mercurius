using Microsoft.AspNetCore.TestHost;
using System.Net;

namespace FunctionalTests.Api;

public class ExceptionInfoEndpointTest
{
    [Fact]
    public async Task CreatingExceptionInfoItemCommandShouldReturnIt()
    {
        // Arrange
        // Act
        Environment.SetEnvironmentVariable("Provider", "InMemory");
        AppFixture application = new AppFixture();

        await application.InitializeAsync();
        var httpClient = application.Host.GetTestClient();
        var result = await httpClient.GetAsync("/api/exceptionInfo");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}
