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
using Api.Authentication;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Ardalis.Result.AspNetCore;
using System.Net;
using Ardalis.Result;
[assembly: InternalsVisibleTo("FunctionalTests")]

var builder = WebApplication.CreateSlimBuilder(args);
var version1 = new ApiVersion(1);
var version2 = new ApiVersion(2);
builder.Configuration.AddEnvironmentVariables(prefix: "Mercurius_");
ConfigurationManager configuration = builder.Configuration;
builder.Services.Configure<MercuriusOptions>(
    builder.Configuration.GetSection(MercuriusOptions.Mercurius));
builder.Services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
        .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
            .For("POST", HttpStatusCode.Created)
            .For("DELETE", HttpStatusCode.NoContent))
        .For(ResultStatus.Error, HttpStatusCode.InternalServerError)
    ));
builder.Services.AddApplicationSwagger(configuration: configuration)
    .AddConfigurationDataBase(configuration: configuration)
    .AddAuth(configuration: configuration)
    .AddAndConfigLocalization();
builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(Application.Extensions).Assembly);
    options.Discovery.IncludeAssembly(typeof(Infrastructure.Extensions).Assembly);
    options.Discovery.IncludeAssembly(typeof(Domain.Extensions).Assembly);
    options.UseFluentValidation();
    options.UseFluentValidation(RegistrationBehavior.ExplicitRegistration);
});
builder.Services.ConfigureInfrastructureServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new QueryStringApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
   
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
}).EnableApiVersionBinding();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.OperationFilter<SwaggerDefaultValues>();
//    options.OperationFilter<SwaggerLanguageHeader>();
//});
builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
var app = builder.Build();
ApiVersionSet versionSet = app.NewApiVersionSet()
     .HasApiVersion(version1)
     .HasApiVersion(version2)
     .ReportApiVersions()
     .Build();

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseApplicationSwagger(configuration: configuration);
   
}
app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MercuriusContext>>();
    var datebase = await db.CreateDbContextAsync();
    await MercuriusContext.InitializeAsync(datebase);
}
app.MapPost("/api/v{version:apiVersion}/exceptionInfo", async (CreateExceptionInfoItemCommand body, IMessageBus bus) =>
        await bus.InvokeAsync<ExceptionInfoCreatedEvent>(body) is ExceptionInfoCreatedEvent exceptionInfos ?
         Results.Created($"/exceptionInfoItems/{exceptionInfos.Id}", body) : Results.BadRequest()
    ).WithApiVersionSet(versionSet).MapToApiVersion(1)
     .Produces<Result<ExceptionInfoCreatedEvent>>(StatusCodes.Status200OK)
      .Produces<Result>(StatusCodes.Status400BadRequest)
     .WithOpenApi().RequireAuthorization(RolesConstants.RolaAdd);

app.MapGet("/api/v{version:apiVersion}/exceptionInfo", async (IMessageBus bus) => await bus.InvokeAsync<IEnumerable<ExceptionInfoEntitie>>(new GetAllExceptionInfoQuerie()) is IEnumerable<ExceptionInfoEntitie> exceptionInfos
             ? Result<IEnumerable<ExceptionInfoEntitie>>.Success(exceptionInfos)
             : Result<IEnumerable<ExceptionInfoEntitie>>.NotFound()).WithApiVersionSet(versionSet).MapToApiVersion(1)

         .Produces<Result<IEnumerable<ExceptionInfoEntitie>>>(StatusCodes.Status200OK)
       .Produces<Result<IEnumerable<ExceptionInfoEntitie>>>(StatusCodes.Status404NotFound)
       .Produces<Result>(StatusCodes.Status400BadRequest)
       .WithOpenApi().RequireAuthorization(RolesConstants.RolaAdd);

app.MapGet("/api/v{version:apiVersion}/exceptionInfo/{id}", async (Guid id, IMessageBus bus) => await bus.InvokeAsync<ExceptionInfoEntitie>(new GetExceptionInfoByIdQuerie(Id: id)) is ExceptionInfoEntitie item
                ? Result<ExceptionInfoEntitie>.Success(item)
                : Result<ExceptionInfoEntitie>.NotFound()).WithApiVersionSet(versionSet).MapToApiVersion(1)
         .Produces<Result<ExceptionInfoEntitie>>(StatusCodes.Status200OK)
         .Produces<Result>(StatusCodes.Status400BadRequest)
       .Produces<Result<ExceptionInfoEntitie>>(StatusCodes.Status404NotFound).RequireAuthorization(RolesConstants.RolaAdd);

app.UseHttpsRedirection();
app.UseApiExceptionHandling();

return await app.RunOaktonCommands(args);