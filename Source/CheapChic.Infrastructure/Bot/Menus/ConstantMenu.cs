﻿using CheapChic.Data;
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

        public static async Task<ReplyKeyboardModel> MyBotsMenu(CheapChicContext context, Guid ownerId, CancellationToken cancellationToken = default)
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
                .AddButton(MenuText.Retailer.MainMenu.MyAnnouncements)
                .AddRow()
                .AddButton(MenuText.Retailer.MainMenu.AddAnnouncement)
                .Build();
    }
}