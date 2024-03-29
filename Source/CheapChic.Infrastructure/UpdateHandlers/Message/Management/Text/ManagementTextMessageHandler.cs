﻿using CheapChic.Data;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text;

public class ManagementTextHandler : IManagementTextMessageHandler
{
    private readonly CheapChicContext _context;
    private readonly IUserService _userService;
    private readonly IManagementMainMenuStateActivator _managementMainMenuStateActivator;
    private readonly IManagementMainMenuStateHandler _managementMainMenuStateHandler;
    private readonly IAddBotStateHandler _addBotStateHandler;
    private readonly IAddBotNameStateHandler _addBotNameStateHandler;
    private readonly IMyBotsStateHandler _myBotsStateHandler;
    private readonly IMyBotsSettingsStateHandler _myBotsSettingsStateHandler;
    private readonly IMyBotsSettingsCurrencyStateHandler _myBotsSettingsCurrencyStateHandler;
    private readonly IMyBotsSettingsPublishDaysStateHandler _myBotsSettingsPublishDaysStateHandler;
    private readonly IMyBotsSettingsPublishEveryHoursStateHandler _myBotsSettingsPublishEveryHoursStateHandler;

    public ManagementTextHandler(CheapChicContext context, IUserService userService,
        IManagementMainMenuStateActivator managementMainMenuStateActivator,
        IManagementMainMenuStateHandler managementMainMenuStateHandler,
        IAddBotStateHandler addBotStateHandler,
        IAddBotNameStateHandler addBotNameStateHandler,
        IMyBotsStateHandler myBotsStateHandler,
        IMyBotsSettingsStateHandler myBotsSettingsStateHandler,
        IMyBotsSettingsCurrencyStateHandler myBotsSettingsCurrencyStateHandler,
        IMyBotsSettingsPublishDaysStateHandler myBotsSettingsPublishDaysStateHandler,
        IMyBotsSettingsPublishEveryHoursStateHandler myBotsSettingsPublishEveryHoursStateHandler)
    {
        _context = context;
        _userService = userService;
        _managementMainMenuStateActivator = managementMainMenuStateActivator;
        _managementMainMenuStateHandler = managementMainMenuStateHandler;
        _addBotStateHandler = addBotStateHandler;
        _addBotNameStateHandler = addBotNameStateHandler;
        _myBotsStateHandler = myBotsStateHandler;
        _myBotsSettingsStateHandler = myBotsSettingsStateHandler;
        _myBotsSettingsCurrencyStateHandler = myBotsSettingsCurrencyStateHandler;
        _myBotsSettingsPublishDaysStateHandler = myBotsSettingsPublishDaysStateHandler;
        _myBotsSettingsPublishEveryHoursStateHandler = myBotsSettingsPublishEveryHoursStateHandler;
    }

    public async Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        var chatId = message.Chat.Id;
        var text = message.Text!;

        var user = await _context.TelegramUsers
            .AsNoTracking()
            .Where(x => !x.Disabled)
            .Where(x => x.ChatId == chatId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return;
        }

        var userState = await _userService.GetUserState(user.Id, cancellationToken);

        if (userState is null)
        {
            await _managementMainMenuStateActivator.Activate(token, user, null, cancellationToken);
            return;
        }

        IManagementStateHandler stateHandler = userState.State switch
        {
            State.ManagementMainMenu => _managementMainMenuStateHandler,
            State.ManagementAddBotToken => _addBotStateHandler,
            State.ManagementAddBotName => _addBotNameStateHandler,
            State.ManagementMyBots => _myBotsStateHandler,
            State.ManagementMyBotsSettings => _myBotsSettingsStateHandler,
            State.ManagementMyBotsSettingsCurrency => _myBotsSettingsCurrencyStateHandler,
            State.ManagementMyBotsSettingsPublishDays => _myBotsSettingsPublishDaysStateHandler,
            State.ManagementMyBotsSettingsPublishEveryHours => _myBotsSettingsPublishEveryHoursStateHandler,
            _ => null
        };

        if (stateHandler is not null)
        {
            await stateHandler.Handle(token, user, text, userState.Data, cancellationToken);
        }
    }
}