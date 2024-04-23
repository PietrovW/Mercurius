using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Application;

public static class Extensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services,IConfiguration configuration)
    {
        return services;
    }
}
