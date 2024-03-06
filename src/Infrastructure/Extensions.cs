using Domain.Common.Interfaces;
using Infrastructure.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Extensions
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services )
    {
        services.AddSingleton<IExceptionInfoRepository, ExceptionInfoRepository>();
    }
}
