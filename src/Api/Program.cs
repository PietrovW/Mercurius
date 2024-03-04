using Api.Providers;
using Api.Options;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Oakton;
using Wolverine;
using Wolverine.FluentValidation;


var builder = WebApplication.CreateSlimBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix: "Mercurius_");
builder.Services.Configure<MercuriusOptions>(
    builder.Configuration.GetSection(MercuriusOptions.Mercurius));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
builder.Services.AddDbContext<MercuriusContext>((services, options) =>
{
    var mercuriusOption = services.GetRequiredService<MercuriusOptions>();
    var provider = config.GetValue("provider", Provider.Postgres.Name);
    if (provider == Provider.Postgres.Name)
    {
        options.UseNpgsql(
            config.GetConnectionString(Provider.Postgres.Name)!,
            x => x.MigrationsAssembly(Provider.Postgres.Assembly)
            );
    }
    if (provider == Provider.MySql.Name)
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
        options.UseMySql(connectionString: config.GetConnectionString(Provider.MySql.Name)!, serverVersion: serverVersion,
                         mySqlOptionsAction: x => x.MigrationsAssembly(Provider.MySql.Assembly)
                );
    }

});


builder.Host.UseWolverine(opts =>
{
    opts.UseFluentValidation();
    opts.UseFluentValidation(RegistrationBehavior.ExplicitRegistration);
});


var app = builder.Build();






if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
return await app.RunOaktonCommands(args);


