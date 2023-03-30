using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

public interface IAddAdDescriptionStateActivator : IRetailerStateActivator
{
}

public class AddAdDescriptionStateActivator : IAddAdDescriptionStateActivator
{
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;

    public AddAdDescriptionStateActivator(ITelegramBot telegramBot, IUserService userService)
    {
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task Activate(TelegramBotEntity bot, TelegramUserEntity user, object stateData,
        CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Retailer.AddAd.SendDescription,
            ConstantMenu.Retailer.AddAnnouncementDescriptionMenu);

        await _telegramBot.SendReplyKeyboard(bot.Token, request, cancellationToken);
        await _userService.SetUserState(user.Id, bot.Id, State.RetailerAddAdDescription, stateData,
            cancellationToken);
    }
}