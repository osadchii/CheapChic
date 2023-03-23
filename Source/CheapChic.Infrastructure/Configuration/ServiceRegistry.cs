using CheapChic.Data;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.Handlers.Telegram.Commands;
using CheapChic.Infrastructure.HostedServices;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.CallbackQuery;
using CheapChic.Infrastructure.UpdateHandlers.Message;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text;
using CheapChic.Infrastructure.UpdateHandlers.MyChatMember;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CheapChic.Infrastructure.Configuration;

public static class ServiceRegistry
{
    private const string DefaultConnectionStringName = "Default";
    
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CheapChicContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(DefaultConnectionStringName);
            options.UseNpgsql(connectionString,
                builder => { builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
        });

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

        services.AddTransient<ICallbackQueryHandler, CallbackQueryHandler>();
        
        services.AddTransient<IMessageHandler, MessageHandler>();
        services.AddTransient<ITextMessageHandler, TextMessageHandler>();
        services.AddTransient<IManagementTextMessageHandler, ManagementTextHandler>();
        services.AddTransient<IRetailerTextMessageHandler, RetailerTextMessageHandler>();

        services.AddTransient<IMainMenuStateActivator, MainMenuStateActivator>();
        services.AddTransient<IMainMenuStateHandler, MainMenuStateHandler>();

        services.AddTransient<IAddBotStateActivator, AddBotStateActivator>();
        services.AddTransient<IAddBotStateHandler, AddBotStateHandler>();
        services.AddTransient<IAddBotNameStateActivator, AddBotNameStateActivator>();
        services.AddTransient<IAddBotNameStateHandler, AddBotNameStateHandler>();
        
        services.AddTransient<IMyChatMemberHandler, MyChatMemberHandler>();

        services.AddTransient<ITelegramBot, TelegramBot>();
        services.AddTransient<IUserService, UserService>();
    }
}