using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;

public interface IManagementMainMenuStateActivator : IManagementStateActivator
{
}

public class ManagementMainMenuStateActivator : IManagementMainMenuStateActivator
{
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;

    public ManagementMainMenuStateActivator(ITelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task Activate(string token, TelegramUserEntity user, object stateData, CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Management.Common.SelectAMenuItem,
            ConstantMenu.Management.MainMenu);

        await _telegramBot.SendReplyKeyboard(token, request, cancellationToken);
        await _userService.SetUserState(user.Id, State.ManagementMainMenu, stateData, cancellationToken);
    }
}