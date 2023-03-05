namespace CheapChic.Api.Configuration;

public static class WebApplicationConfigurator
{
    public static void ConfigureApplication(this WebApplication application)
    {
        application.UseHsts();
        application.UseRouting();
        application.MapControllers();
    }
}