using Prometheus;

namespace CheapChic.Api.Configuration;

public static class WebApplicationConfigurator
{
    public static void ConfigureApplication(this WebApplication application)
    {
        application.UseHsts();
        application.UseRouting();
        application.UseHttpMetrics();
#pragma warning disable ASP0014
        application.UseEndpoints(x => { x.MapMetrics(); });
#pragma warning restore ASP0014
        application.MapControllers();
    }
}