using CheapChic.Data;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.Handlers.Telegram.Commands;
using CheapChic.Infrastructure.HostedServices;
using CheapChic.Infrastructure.Services.AdMessageBuilder;
using CheapChic.Infrastructure.Services.PhotoAdService;
using CheapChic.Infrastructure.Services.PhotoService;
using CheapChic.Infrastructure.Services.TelegramBotService;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.CallbackQuery;
using CheapChic.Infrastructure.UpdateHandlers.Message;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Document;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Photo;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Document;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Photo;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MyAds;
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

        services.AddHttpClient<ITelegramBot, TelegramBot>();

        services.AddMemoryCache();

        services.AddHostedService<WebhookHostedService>();
        services.AddHostedService<AdPublisherHostedService>();
        services.AddHostedService<AdDisablerHostedService>();

        services.AddTransient<ICallbackQueryHandler, CallbackQueryHandler>();

        services.AddTransient<IMessageHandler, MessageHandler>();
        services.AddTransient<ITextMessageHandler, TextMessageHandler>();
        services.AddTransient<IPhotoMessageHandler, PhotoMessageHandler>();
        services.AddTransient<IDocumentMessageHandler, DocumentMessageHandler>();

        services.AddTransient<IManagementTextMessageHandler, ManagementTextHandler>();
        services.AddTransient<IRetailerTextMessageHandler, RetailerTextMessageHandler>();

        services.AddTransient<IRetailerDocumentMessageHandler, RetailerDocumentMessageHandler>();
        services.AddTransient<IRetailerPhotoMessageHandler, RetailerPhotoMessageHandler>();

        services.AddTransient<IManagementMainMenuStateActivator, ManagementMainMenuStateActivator>();
        services.AddTransient<IManagementMainMenuStateHandler, ManagementMainMenuStateHandler>();

        services.AddTransient<IAddBotStateActivator, AddBotStateActivator>();
        services.AddTransient<IAddBotStateHandler, AddBotStateHandler>();
        services.AddTransient<IAddBotNameStateActivator, AddBotNameStateActivator>();
        services.AddTransient<IAddBotNameStateHandler, AddBotNameStateHandler>();

        services.AddTransient<IMyBotsStateActivator, MyBotsStateActivator>();
        services.AddTransient<IMyBotsStateHandler, MyBotsStateHandler>();

        services.AddTransient<IMyBotsSettingsStateActivator, MyBotsSettingsStateActivator>();
        services.AddTransient<IMyBotsSettingsStateHandler, MyBotsSettingsStateHandler>();

        services.AddTransient<IMyBotsSettingsCurrencyStateActivator, MyBotsSettingsCurrencyStateActivator>();
        services.AddTransient<IMyBotsSettingsCurrencyStateHandler, MyBotsSettingsCurrencyStateHandler>();

        services.AddTransient<IMyBotsSettingsPublishDaysStateActivator, MyBotsSettingsPublishDaysStateActivator>();
        services.AddTransient<IMyBotsSettingsPublishDaysStateHandler, MyBotsSettingsPublishDaysStateHandler>();

        services
            .AddTransient<IMyBotsSettingsPublishEveryHoursStateActivator,
                MyBotsSettingsPublishEveryHoursStateActivator>();
        services
            .AddTransient<IMyBotsSettingsPublishEveryHoursStateHandler, MyBotsSettingsPublishEveryHoursStateHandler>();

        services.AddTransient<IRetailerMainMenuStateActivator, RetailerMainMenuStateActivator>();
        services.AddTransient<IRetailerMainMenuStateHandler, RetailerMainMenuStateHandler>();

        services.AddTransient<IMyAdsStateActivator, MyAdsStateActivator>();
        services.AddTransient<IMyAdsStateHandler, MyAdsStateHandler>();

        services.AddTransient<IMyAdsSettingsStateActivator, MyAdsSettingsStateActivator>();
        services.AddTransient<IMyAdsSettingsStateHandler, MyAdsSettingsStateHandler>();

        services.AddTransient<IAddAdStateActivator, AddAdStateActivator>();
        services.AddTransient<IAddAdStateHandler, AddAdStateHandler>();

        services.AddTransient<IAddAdNameStateActivator, AddAdNameStateActivator>();
        services.AddTransient<IAddAdNameStateHandler, AddAdNameStateHandler>();

        services
            .AddTransient<IAddAdDescriptionStateActivator,
                AddAdDescriptionStateActivator>();
        services.AddTransient<IAddAdDescriptionStateHandler, AddAdDescriptionStateHandler>();

        services.AddTransient<IAddAdPriceStateActivator, AddAdPriceStateActivator>();
        services.AddTransient<IAddAdPriceStateHandler, AddAdPriceStateHandler>();

        services.AddTransient<IAddAdPhotoStateActivator, AddAdPhotoStateActivator>();
        services.AddTransient<IAddAdPhotoStateHandler, AddAdPhotoStateHandler>();

        services.AddTransient<IAddAdConfirmationStateActivator, AddAdConfirmationStateActivator>();
        services.AddTransient<IAddAdConfirmationStateHandler, AddAdConfirmationStateHandler>();

        services.AddTransient<IMyChatMemberHandler, MyChatMemberHandler>();

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITelegramBotService, TelegramBotService>();
        services.AddTransient<IPhotoService, PhotoService>();
        services.AddTransient<IPhotoAdService, PhotoAdService>();
        services.AddTransient<IAdMessageBuilder, AdMessageBuilder>();
    }
}