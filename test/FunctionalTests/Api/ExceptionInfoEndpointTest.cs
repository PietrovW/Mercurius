using Application.Entities;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;

namespace FunctionalTests.Api;

public class ExceptionInfoEndpointTest
{

    [Fact]
    public async Task GetAllExceptionInfoItemCommandShouldReturnStatusCodeOK()
    {
        // Arrange
        // Act
        var application = new AppFixture();

        await application.InitializeAsync();
        var httpClient = application.Host.GetTestClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        var result = await httpClient.GetAsync("/api/exceptionInfo");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
    
    [Fact]
    public async Task CreatingExceptionInfoItemCommandShouldReturnIt()
    {
        // Arrange
        var Id = Guid.NewGuid();
        var exceptionInfo = new ExceptionInfoEntitie() {
            Id = Guid.NewGuid(),
            InnerException = $"InnerException_{Id} ",
            Message = $"Message_{Id} ",
            Source = $"Source_{Id} ",
            StackTrace = $"StackTrace_{Id} ",
            TargetSite = $"TargetSite_{Id} ",
        };
        // Act
        AppFixture application = new AppFixture();

        await application.InitializeAsync();
        var httpClient = application.Host.GetTestClient();
        var result = await httpClient.PostAsJsonAsync<ExceptionInfoEntitie>("/api/exceptionInfo",value: exceptionInfo);

        // Assert
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
    }

    [Fact]
    public async Task GetByIdExceptionInfoItemCommandShouldReturnStatusCodeOK()
    {
        // Arrange
        Guid Id = DataSeeder.Id;
        // Act
        AppFixture application = new AppFixture();

        await application.InitializeAsync();
        var httpClient = application.Host.GetTestClient();
        var result = await httpClient.GetAsync($"/api/exceptionInfo/{Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task GetByIdExceptionInfoItemCommandShouldReturnStatusCodeNotFound()
    {
        // Arrange
        Guid Id = Guid.NewGuid();
        // Act
        AppFixture application = new AppFixture();

        await application.InitializeAsync();
        var httpClient = application.Host.GetTestClient();
        var result = await httpClient.GetAsync($"/api/exceptionInfo/{Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}
