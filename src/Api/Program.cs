using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Oakton;
using Wolverine;
using Wolverine.FluentValidation;
using Application.ExceptionInfos.Commands.CreateExceptionInfoItem;
using Infrastructure;
using Application.Entities;
using Application.ExceptionInfos.Queries.GetAllExceptionInfo;
using Application.ExceptionInfos.Queries.GeExceptionInfoById;
using System.Runtime.CompilerServices;
using Domain.Events;
using Api.Extensions;
using Api.Options;

[assembly: InternalsVisibleTo("FunctionalTests")]

var builder = WebApplication.CreateSlimBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "Mercurius_");
ConfigurationManager configuration = builder.Configuration;
builder.Services.Configure<KeycloakOptions>(
    builder.Configuration.GetSection(KeycloakOptions.Keycloak));
builder.Services.Configure<MercuriusOptions>(
    builder.Configuration.GetSection(MercuriusOptions.Mercurius));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationSwagger(configuration: configuration)
    .AddConfigurationDataBase(configuration: configuration)
    .AddAuth(configuration: configuration);
builder.Services.AddAuthentication();
builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(Application.Extensions).Assembly);
    options.Discovery.IncludeAssembly(typeof(Infrastructure.Extensions).Assembly);
    options.Discovery.IncludeAssembly(typeof(Domain.Extensions).Assembly);

    options.UseFluentValidation();
    options.UseFluentValidation(RegistrationBehavior.ExplicitRegistration);
});
builder.Services.ConfigureInfrastructureServices();

var app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseApplicationSwagger(configuration: configuration);
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MercuriusContext>>();
    var datebase = await db.CreateDbContextAsync();
    await MercuriusContext.InitializeAsync(datebase);
}

app.MapPost("/api/exceptionInfo", async (CreateExceptionInfoItemCommand body, IMessageBus bus) =>
    await bus.InvokeAsync<ExceptionInfoCreatedEvent>(body) is ExceptionInfoCreatedEvent exceptionInfos ?
     Results.Created($"/exceptionInfoItems/{exceptionInfos.Id}", body) : Results.BadRequest()
).WithOpenApi().RequireAuthorization();

app.MapGet("/api/exceptionInfo/", async (IMessageBus bus) => await bus.InvokeAsync<IEnumerable<ExceptionInfoEntitie>>(new GetAllExceptionInfoQuerie()) is IEnumerable<ExceptionInfoEntitie> exceptionInfos
         ? Results.Ok(exceptionInfos)
         : Results.NotFound())
     .Produces<ExceptionInfoEntitie>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound)
   .WithOpenApi(operation => new(operation)
   {
       OperationId = "GetExceptionInfo"
   }).RequireAuthorization();

app.MapGet("/api/exceptionInfo/{id}", async (Guid id, IMessageBus bus) => await bus.InvokeAsync<ExceptionInfoEntitie>(new GetExceptionInfoByIdQuerie(Id: id)) is ExceptionInfoEntitie item
            ? Results.Ok(item)
            : Results.NotFound())
     .Produces<ExceptionInfoEntitie>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound).RequireAuthorization();

app.UseHttpsRedirection();
return await app.RunOaktonCommands(args);
