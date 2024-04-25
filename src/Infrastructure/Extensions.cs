using Domain.Common.Interfaces;
using Infrastructure.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Extensions
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services )
    {
        services.AddSingleton<IExceptionInfoRepository, ExceptionInfoRepository>();
        
        return services;
    }
}
