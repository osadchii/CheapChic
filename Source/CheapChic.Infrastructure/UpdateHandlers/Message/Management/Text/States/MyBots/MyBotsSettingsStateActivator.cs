using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots.Models;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

public interface IMyBotsSettingsStateActivator : IManagementStateActivator
{
    
}

public class MyBotsSettingsStateActivator : IMyBotsSettingsStateActivator
{
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;
    private readonly CheapChicContext _context;

    public MyBotsSettingsStateActivator(ITelegramBot telegramBot, IUserService userService, CheapChicContext context)
    {
        _telegramBot = telegramBot;
        _userService = userService;
        _context = context;
    }

    public async Task Activate(string token, TelegramUserEntity user, object stateData, CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Management.Common.SelectAMenuItem,
            await ConstantMenu.Management.MyBotsSettingsMenu(_context, (stateData as MyBotStateData)!.BotId, cancellationToken));

        await _telegramBot.SendReplyKeyboard(token, request, cancellationToken);
        await _userService.SetUserState(user.Id, State.ManagementMyBotsSettings, stateData, cancellationToken);
    }
}