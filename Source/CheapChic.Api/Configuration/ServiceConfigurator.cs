namespace CheapChic.Api.Configuration;

public static class ServiceConfigurator
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
    }
}