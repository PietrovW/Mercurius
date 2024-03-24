using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Extensions
{
    public static services ConfigureServices(this IServiceCollection services,IConfiguration configuration)
    {
        return services;
    }
}
