using Alba;
using Api.Authorization.Decision;
using Api.Providers;
using FluentAssertions.Common;
using FunctionalTests.AuthHandlerTest;
using Infrastructure.Data;
using JasperFx.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Oakton;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;

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
                //services.AddAuthentication(defaultScheme: "TestScheme")
                //    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                //        "TestScheme", options => { });
                // services.AddSingleton<IAuthorizationHandler, DecisionRequirementHandler>();
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                })
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

              //  services.AddAuthentication(defaultScheme: "TestScheme")
                //     .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
            });

            x.ConfigureServices(services =>
             {
                // var dbConnectionDescriptor = services.SingleOrDefault(
                //d => d.ServiceType ==
                //    typeof(AuthenticationHandler<JwtBearerOptions>));

                // services.Remove(dbConnectionDescriptor);
                // services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                //   .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });

                // services.AddSingleton<IAuthorizationHandler, TestAuthorizationHandler>();
                // services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = "teste");
                 //services.RemoveAll<IPostConfigureOptions<JwtBearerOptions>>();
                 //services.PostConfigure<JwtBearerOptions>(options =>
                 //{
                 //    options.TokenValidationParameters = new TokenValidationParameters()
                 //    {
                 //        SignatureValidator = (token, parameters) => new JwtSecurityToken(token)
                 //    };
                 //    //options.Audience = TestAuthorisationConstants.Audience;
                 //    //options.Authority = TestAuthorisationConstants.Issuer;
                 //    //options.BackchannelHttpHandler = new MockBackchannel();
                 //    //options.MetadataAddress = "https://inmemory.microsoft.com/common/.well-known/openid-configuration";
                 //});
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
