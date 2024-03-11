using Api.Providers;
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
using Microsoft.OpenApi.Models;
using System.Runtime.CompilerServices;
using Domain.Events;
[assembly: InternalsVisibleTo("FunctionalTests")]

var builder = WebApplication.CreateSlimBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix: "Mercurius_");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mercurius API", Version = "v1" });
});
var config = builder.Configuration;

builder.Services.AddDbContextFactory<MercuriusContext>(options =>
{
    var provider = config.GetValue("provider", Provider.Postgres.Name);
    if (provider == Provider.Postgres.Name)
    {
        options.UseNpgsql(
            connectionString: config.GetConnectionString(Provider.Postgres.Name)!,
           npgsqlOptionsAction: x => x.MigrationsAssembly(Provider.Postgres.Assembly));
    }
    else if (provider == Provider.MySql.Name)
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
        options.UseMySql(connectionString: config.GetConnectionString(Provider.MySql.Name)!, serverVersion: serverVersion,
                         mySqlOptionsAction: x => x.MigrationsAssembly(Provider.MySql.Assembly));
    }
    else if (provider == Provider.SqlServer.Name)
    {
        options.UseSqlServer(connectionString: config.GetConnectionString(Provider.SqlServer.Name)!,
                          sqlServerOptionsAction: x => x.MigrationsAssembly(Provider.SqlServer.Assembly));
    }
    else if (provider == Provider.InMemory.Name)
    {
        options.UseInMemoryDatabase($"MercuriusDatabase-{Guid.NewGuid()}");
    }
    else
    {
        throw new Exception($"Unsupported provider: {provider}");
    }
});

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MercuriusContext>>();
    var datebase = await db.CreateDbContextAsync();
    await MercuriusContext.InitializeAsync(datebase);
}

app.MapPost("/api/exceptionInfo", async (CreateExceptionInfoItemCommand body, IMessageBus bus) =>
    await bus.InvokeAsync<ExceptionInfoCreatedEvent>(body) is ExceptionInfoCreatedEvent exceptionInfos ?
     Results.Created($"/exceptionInfoItems/{exceptionInfos.Id}", body) : Results.BadRequest()
).WithOpenApi();

app.MapGet("/api/exceptionInfo/", async (IMessageBus bus) => await bus.InvokeAsync<IEnumerable<ExceptionInfoEntitie>>(new GetAllExceptionInfoQuerie()) is IEnumerable<ExceptionInfoEntitie> exceptionInfos
         ? Results.Ok(exceptionInfos)
         : Results.NotFound())
     .Produces<ExceptionInfoEntitie>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound)
   .WithOpenApi(operation => new(operation)
   {
       OperationId = "GetExceptionInfo"
   });

app.MapGet("/api/exceptionInfo/{id}", async (Guid id, IMessageBus bus) => await bus.InvokeAsync<ExceptionInfoEntitie>(new GetExceptionInfoByIdQuerie(Id: id)) is ExceptionInfoEntitie item
            ? Results.Ok(item)
            : Results.NotFound())
     .Produces<ExceptionInfoEntitie>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status404NotFound);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mercurius API V1");
    });
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

app.UseHttpsRedirection();
 return await app.RunOaktonCommands(args);
