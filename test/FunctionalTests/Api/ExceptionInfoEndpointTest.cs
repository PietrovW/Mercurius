﻿using Application.Entities;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http.Headers;
using FunctionalTests.AuthHandlerTest;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FunctionalTests.Api;

public class ExceptionInfoEndpointTest
{
    [Fact]
    public async Task GetAllExceptionInfoItemCommandShouldReturnStatusCodeOK()
    {
        // Arrange
        var jwt = CreateTestJwt("rola_add");
        var application = new AppFixture();
        await application.InitializeAsync();
        var httpClient = application.Host.GetTestClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: TestAuthHandler.AuthenticationScheme);
        // Act
        var result = await httpClient.GetAsync("/api/exceptionInfo");

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task CreatingExceptionInfoItemCommandShouldReturnIt()
    {
        // Arrange
        var Id = Guid.NewGuid();
        var exceptionInfo = new ExceptionInfoEntitie()
        {
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
        var result = await httpClient.PostAsJsonAsync<ExceptionInfoEntitie>("/api/exceptionInfo", value: exceptionInfo);

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

    private static string CreateTestJwt(string role)
    {
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(RSA.Create()), SecurityAlgorithms.RsaSha512),
            Subject = new ClaimsIdentity(new List<Claim>
        {
            new("name", "Some User"), new("role", role)
        })
        };

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var token = securityTokenHandler.CreateToken(securityTokenDescriptor);
        var encodedAccessToken = securityTokenHandler.WriteToken(token);

        return encodedAccessToken;
    }
}
