using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;

public interface IManagementAddBotStateActivator : IManagementStateActivator
{
}

public class ManagementAddBotStateActivator : IManagementAddBotStateActivator
{
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;

    public ManagementAddBotStateActivator(ITelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task Activate(string token, TelegramUserEntity user, object stateData, CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Management.AddBot.SendToken,
            ConstantMenu.Management.AddBotMenu);

        await _telegramBot.SendReplyKeyboard(token, request, cancellationToken);
        await _userService.SetUserState(user.Id, State.ManagementAddBotToken, stateData, cancellationToken);
    }
}