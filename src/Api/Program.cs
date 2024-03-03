using Api.Options;
using Infrastructure.Data;
using Infrastructure.Data.Providers;
using Microsoft.EntityFrameworkCore;
using Oakton;
using System.Data.SqlTypes;
using Wolverine;
using Wolverine.FluentValidation;


var builder = WebApplication.CreateSlimBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix: "Mercurius_");
builder.Services.Configure<MercuriusOptions>(
    builder.Configuration.GetSection(MercuriusOptions.Mercurius));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
builder.Services.AddDbContext<MercuriusContext>(options =>
{
    var provider = config.GetValue("provider", Provider.Postgres.Name);
    if (provider == Provider.Postgres.Name)
    {
    options.UseNpgsql(
        config.GetConnectionString(Provider.Postgres.Name)!,
        x => x.MigrationsAssembly(Provider.Postgres.Assembly)
           // options.UseNpgsql(connectionString: mercuriusOption.ConnectionString);//,
//                          // npgsqlOptionsAction: x => x.MigrationsAssembly(Provider.Postgres.Assembly));
        );
    }
    
});
//builder.Services.AddDbContextFactory<MercuriusContext>(
//                  (services, options) =>
//                  {
//                      var provider = config.GetValue("provider", Provider.Postgres.Name);
//                      var mercuriusOption = services.GetRequiredService<MercuriusOptions>();
//                    //  var provider = mercuriusOption.Provider;
//                      if (provider == Provider.Postgres.Name)
//                      {
//                      options.UseNpgsql(connectionString: mercuriusOption.ConnectionString);//,
//                          // npgsqlOptionsAction: x => x.MigrationsAssembly(Provider.Postgres.Assembly));
//                      }
//                      if (provider == Provider.MySql.Name)
//                      {
//                          options.UseMySql(connectionString: mercuriusOption.ConnectionString, serverVersion: ServerVersion.AutoDetect(connectionString: mercuriusOption.ConnectionString),
//                         mySqlOptionsAction: x => x.MigrationsAssembly(Provider.MySql.Assembly));
//                      }

//                  });

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


