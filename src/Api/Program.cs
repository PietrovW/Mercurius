using Api.Providers;
using Api.Options;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Oakton;
using Wolverine;
using Wolverine.FluentValidation;


var builder = WebApplication.CreateSlimBuilder(args);
//builder.Configuration.AddEnvironmentVariables(prefix: "Mercurius_");
//builder.Services.Configure<MercuriusOptions>(
//    builder.Configuration.GetSection(MercuriusOptions.Mercurius));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
builder.Services.AddPooledDbContextFactory<MercuriusContext>(options => options.UseNpgsql(
            connectionString: config.GetConnectionString(Provider.Postgres.Name)!,
          npgsqlOptionsAction: x => x.MigrationsAssembly(Provider.Postgres.Assembly)));
//builder.Services.AddDbContext<MercuriusContext>( options =>
//{
//    //var mercuriusOption = services.GetRequiredService<MercuriusOptions>();
//    var provider = Provider.Postgres.Name;// config.GetValue("provider", Provider.Postgres.Name);

//    if (provider == Provider.Postgres.Name)
//    {
//        options.UseNpgsql(
//            connectionString: config.GetConnectionString(Provider.Postgres.Name)!,
//           npgsqlOptionsAction: x => x.MigrationsAssembly(Provider.Postgres.Assembly));
//    }
//    else if (provider == Provider.MySql.Name)
//    {
//        var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
//        options.UseMySql(connectionString: config.GetConnectionString(Provider.MySql.Name)!, serverVersion: serverVersion,
//                         mySqlOptionsAction: x => x.MigrationsAssembly(Provider.MySql.Assembly));
//    }
//    else if (provider == Provider.SqlServer.Name)
//    {


//        options.UseNpgsql(
//           connectionString: config.GetConnectionString(Provider.Postgres.Name)!,
//          npgsqlOptionsAction: x => x.MigrationsAssembly(Provider.Postgres.Assembly));
//        //options.UseSqlServer(connectionString: mercuriusOption.ConnectionString,
//        //                  sqlServerOptionsAction: x => x.MigrationsAssembly(Provider.SqlServer.Assembly));
//    }
//    else
//    {
//        throw new Exception($"Unsupported provider: {provider}");
//    }
//});


builder.Host.UseWolverine(opts =>
{
    opts.UseFluentValidation();
    opts.UseFluentValidation(RegistrationBehavior.ExplicitRegistration);
});


var app = builder.Build();
var dbContext = app.Services.GetRequiredService<MercuriusContext>();
await dbContext.Database.MigrateAsync();
// initialize database
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<MercuriusContext>();
//    await MercuriusContext.InitializeAsync(db);
//}
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    //var persistedGrantDbContextFactory = new PersistedGrantDbContextFactory();

    //PersistedGrantDbContext persistedGrantDbContext = persistedGrantDbContextFactory.CreateDbContext(null);
    //await persistedGrantDbContext.Database.MigrateAsync();

    // Additional migrations

            }

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
return await app.RunOaktonCommands(args);


