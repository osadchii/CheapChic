using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MyAds;

public interface IMyAdsStateActivator : IRetailerStateActivator
{
}

public class MyAdsStateActivator : IMyAdsStateActivator
{
    private readonly CheapChicContext _context;
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;

    public MyAdsStateActivator(CheapChicContext context, ITelegramBot telegramBot, IUserService userService)
    {
        _context = context;
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task Activate(TelegramBotEntity bot, TelegramUserEntity user, object stateData,
        CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Retailer.Common.SelectAMenuItem,
            await ConstantMenu.Retailer.MyAdsMenu(_context, bot.Id, user.Id, cancellationToken));

        await _telegramBot.SendReplyKeyboard(bot.Token, request, cancellationToken);
        await _userService.SetUserState(user.Id, bot.Id, State.RetailerMyAds, stateData, cancellationToken);
    }
}