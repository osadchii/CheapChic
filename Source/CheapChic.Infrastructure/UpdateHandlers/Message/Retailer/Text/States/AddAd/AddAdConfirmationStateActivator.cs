using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.AdMessageBuilder;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

public interface IAddAdConfirmationStateActivator : IRetailerStateActivator
{
}

public class AddAdConfirmationStateActivator : IAddAdConfirmationStateActivator
{
    private readonly IAdMessageBuilder _adMessageBuilder;
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;

    public AddAdConfirmationStateActivator(IAdMessageBuilder adMessageBuilder, ITelegramBot telegramBot, IUserService userService)
    {
        _adMessageBuilder = adMessageBuilder;
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task Activate(TelegramBotEntity bot, TelegramUserEntity user, object stateData,
        CancellationToken cancellationToken = default)
    {
        var state = stateData as AddAdStateData;
        var adRequest = _adMessageBuilder.BuildByState(state, user.ChatId, user.Username, bot.Currency);

        switch (adRequest)
        {
            case SendTextMessageRequest textRequest:
                await _telegramBot.SendText(bot.Token, textRequest, cancellationToken);
                break;
            case SendMediaGroupRequest mediaRequest:
                await _telegramBot.SendPhoto(bot.Token, mediaRequest, cancellationToken);
                break;
        }
        
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Retailer.AddAd.AdConfirmation,
            ConstantMenu.Retailer.AddAnnouncementConfirmationMenu);

        await _telegramBot.SendReplyKeyboard(bot.Token, request, cancellationToken);
        await _userService.SetUserState(user.Id, bot.Id, State.RetailerAddAdConfirmation, stateData,
            cancellationToken);
    }
}