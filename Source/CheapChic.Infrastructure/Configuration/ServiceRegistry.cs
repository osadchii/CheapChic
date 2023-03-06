using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.Handlers.Telegram.Commands;
using CheapChic.Infrastructure.HostedServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CheapChic.Infrastructure.Configuration;

public static class ServiceRegistry
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(mediatrConfiguration =>
        {
            mediatrConfiguration.RegisterServicesFromAssemblyContaining<HandleUpdate.Command>();
        });

        services.Configure<ApplicationOptions>(
            configuration.GetSection(ApplicationOptions.OptionName));
        services.Configure<ManagementBotOptions>(
            configuration.GetSection(ManagementBotOptions.OptionName));

        services.AddHttpClient();

        services.AddHostedService<WebhookHostedService>();
    }
}