using Api.Providers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Oakton;
using Wolverine;
using Wolverine.FluentValidation;
using Application.ExceptionInfos.Commands.CreateExceptionInfoItem;
using Infrastructure;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix: "Mercurius_");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
builder.Services.AddPooledDbContextFactory<MercuriusContext>(options => options.UseNpgsql(
            connectionString: config.GetConnectionString(Provider.Postgres.Name)!,
          npgsqlOptionsAction: x => x.MigrationsAssembly(Provider.Postgres.Assembly)));
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

app.MapPost("/exceptionInfoItems", async (CreateExceptionInfoItemCommand body, IMessageBus bus) =>
{
    await bus.InvokeAsync(body);
    return Results.Created($"/exceptionInfoItems/1", body);
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseHttpsRedirection();
return await app.RunOaktonCommands(args);


