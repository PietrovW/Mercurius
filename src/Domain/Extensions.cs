using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class Extensions
{
    public static IServiceCollection ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
            return services;
    }
}

