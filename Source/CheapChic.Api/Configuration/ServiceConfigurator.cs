using CheapChic.Infrastructure.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CheapChic.Api.Configuration;

public static class ServiceConfigurator
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        services.AddInfrastructure(configuration);
    }
}