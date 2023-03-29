using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;

public interface IAddBotNameStateActivator : IManagementStateActivator
{
}

public class AddBotNameStateActivator : IAddBotNameStateActivator
{
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;

    public AddBotNameStateActivator(ITelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }
    
    public async Task Activate(string token, TelegramUserEntity user, object stateData, CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Management.AddBot.SendName,
            ConstantMenu.Management.AddBotNameMenu);

        await _telegramBot.SendReplyKeyboard(token, request, cancellationToken);
        await _userService.SetUserState(user.Id, State.ManagementAddBotName, stateData, cancellationToken);
    }
}