using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

public interface IMyBotsSettingsPublishEveryHoursStateActivator : IManagementStateActivator
{
}

public class MyBotsSettingsPublishEveryHoursStateActivator : IMyBotsSettingsPublishEveryHoursStateActivator
{
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;

    public MyBotsSettingsPublishEveryHoursStateActivator(ITelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task Activate(string token, TelegramUserEntity user, object stateData,
        CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Management.MyBots.SendPublishEveryHours,
            ConstantMenu.Management.MyBotsSettingsPublishEveryHoursMenu);

        await _telegramBot.SendReplyKeyboard(token, request, cancellationToken);
        await _userService.SetUserState(user.Id, State.ManagementMyBotsSettingsPublishEveryHours, stateData,
            cancellationToken);
    }
}