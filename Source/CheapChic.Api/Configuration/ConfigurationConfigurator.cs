namespace CheapChic.Api.Configuration;

public static class ConfigurationConfigurator
{
    private const string AppSettingsFile = "appsettings.json";
    private const string AppSettingsLocalFile = "appsettings.local.json";

    public static void ConfigureConfiguration(this ConfigurationManager configurationManager)
    {
        configurationManager
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(AppSettingsFile, optional: false)
            .AddJsonFile(AppSettingsLocalFile, optional: true)
            .AddEnvironmentVariables();
    }
}