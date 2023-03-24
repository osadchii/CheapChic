using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;

public interface IRetailerMainMenuStateActivator : IRetailerStateActivator
{
}

public class RetailerMainMenuStateActivator : IRetailerMainMenuStateActivator
{
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;

    public RetailerMainMenuStateActivator(ITelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task Activate(TelegramBotEntity bot, TelegramUserEntity user, object stateData,
        CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Retailer.Common.SelectAMenuItem,
            ConstantMenu.Retailer.MainMenu);

        await _telegramBot.SendReplyKeyboard(bot.Token, request, cancellationToken);
        await _userService.SetUserState(user.Id, bot.Id, State.RetailerMainMenu, stateData, cancellationToken);
    }
}