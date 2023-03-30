using CheapChic.Data;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.Bot.Menus;

public static class ConstantMenu
{
    public static class Management
    {
        public static ReplyKeyboardModel MainMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Management.MainMenu.AddBot)
                .AddButton(MenuText.Management.MainMenu.MyBots)
                .Build();

        public static async Task<ReplyKeyboardModel> MyBotsMenu(CheapChicContext context, Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var builder = ReplyKeyboardBuilder.Create();

            var botNames = await context.TelegramBots
                .Where(x => x.OwnerId == ownerId)
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToListAsync(cancellationToken);

            foreach (var botName in botNames)
            {
                builder.AddRow()
                    .AddButton(botName);
            }

            builder.AddRow()
                .AddButton(MenuText.Management.Common.Back);

            return builder.Build();
        }

        public static async Task<ReplyKeyboardModel> MyBotsSettingsMenu(CheapChicContext context, Guid botId,
            CancellationToken cancellationToken = default)
        {
            var builder = ReplyKeyboardBuilder
                .Create();

            var botSettings = await context.TelegramBots
                .AsNoTracking()
                .Where(x => x.Id == botId)
                .Select(x => new { x.Disabled })
                .FirstOrDefaultAsync(cancellationToken);

            builder = builder
                .AddRow()
                .AddButton(botSettings.Disabled
                    ? MenuText.Management.MyBotsMenu.Enable
                    : MenuText.Management.MyBotsMenu.Disable)
                .AddRow()
                .AddButton(MenuText.Management.MyBotsMenu.Currency)
                .AddRow()
                .AddButton(MenuText.Management.MyBotsMenu.PublishDays)
                .AddRow()
                .AddButton(MenuText.Management.MyBotsMenu.PublishEvery);

            return builder
                .AddRow()
                .AddButton(MenuText.Management.Common.Back)
                .Build();
        }

        public static ReplyKeyboardModel MyBotsSettingsCurrencyMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Management.Common.Back)
                .Build();

        public static ReplyKeyboardModel MyBotsSettingsPublishDaysMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Management.Common.Back)
                .Build();

        public static ReplyKeyboardModel MyBotsSettingsPublishEveryHoursMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Management.Common.Back)
                .Build();

        public static ReplyKeyboardModel AddBotMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Management.Common.Back)
                .Build();

        public static ReplyKeyboardModel AddBotNameMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Management.Common.Back)
                .Build();
    }

    public static class Retailer
    {
        public static ReplyKeyboardModel MainMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Retailer.MainMenu.MyAds)
                .AddRow()
                .AddButton(MenuText.Retailer.MainMenu.AddAd)
                .Build();

        public static async Task<ReplyKeyboardModel> MyAdsMenu(CheapChicContext context, Guid botId, Guid userId,
            CancellationToken cancellationToken = default)
        {
            var builder = ReplyKeyboardBuilder.Create();

            var adNames = await context.Ads
                .Where(x => x.BotId == botId)
                .Where(x => x.UserId == userId)
                .Where(x => !x.Disable)
                .Select(x => x.Name)
                .OrderBy(x => x)
                .ToListAsync(cancellationToken);

            foreach (var botName in adNames)
            {
                builder.AddRow()
                    .AddButton(botName);
            }

            builder.AddRow()
                .AddButton(MenuText.Retailer.Common.Back);

            return builder.Build();
        }

        public static ReplyKeyboardModel MyAdSettingsMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Retailer.MyAdsMenu.Disable)
                .AddRow()
                .AddButton(MenuText.Retailer.Common.Back)
                .Build();

        public static ReplyKeyboardModel AddAnnouncementMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Retailer.AddAdMenu.Sell)
                .AddButton(MenuText.Retailer.AddAdMenu.Buy)
                .AddRow()
                .AddButton(MenuText.Retailer.AddAdMenu.OfferAService)
                .AddButton(MenuText.Retailer.AddAdMenu.LookingForAService)
                .AddRow()
                .AddButton(MenuText.Retailer.Common.Back)
                .Build();

        public static ReplyKeyboardModel AddAnnouncementNameMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Retailer.Common.Back)
                .Build();

        public static ReplyKeyboardModel AddAnnouncementDescriptionMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Retailer.Common.Back)
                .Build();

        public static ReplyKeyboardModel AddAnnouncementPriceMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Retailer.AddAdPriceMenu.Negotiated)
                .AddRow()
                .AddButton(MenuText.Retailer.Common.Back)
                .Build();

        public static ReplyKeyboardModel AddAnnouncementPhotoMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Retailer.AddAdPhotoMenu.ClearPhotos)
                .AddRow()
                .AddButton(MenuText.Retailer.AddAdPhotoMenu.Done)
                .AddRow()
                .AddButton(MenuText.Retailer.Common.Back)
                .Build();

        public static ReplyKeyboardModel AddAnnouncementConfirmationMenu =>
            ReplyKeyboardBuilder
                .Create()
                .AddRow()
                .AddButton(MenuText.Retailer.AddAdConfirmationMenu.Publish)
                .AddRow()
                .AddButton(MenuText.Retailer.Common.Back)
                .Build();
    }
}