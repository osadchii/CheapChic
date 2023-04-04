using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MyAds;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;

public interface IRetailerMainMenuStateHandler : IRetailerStateHandler
{
}

public class RetailerMainMenuStateHandler : IRetailerMainMenuStateHandler
{
    private readonly IAddAdStateActivator _addAdStateActivator;
    private readonly IMyAdsStateActivator _myAdsStateActivator;
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;
    private readonly ITelegramBot _telegramBot;

    public RetailerMainMenuStateHandler(IRetailerMainMenuStateActivator retailerMainMenuStateActivator,
        IAddAdStateActivator addAdStateActivator, IMyAdsStateActivator myAdsStateActivator, ITelegramBot telegramBot)
    {
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
        _addAdStateActivator = addAdStateActivator;
        _myAdsStateActivator = myAdsStateActivator;
        _telegramBot = telegramBot;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Retailer.MainMenu.AddAd:
                if (string.IsNullOrEmpty(user.Username))
                {
                    var setUsernameRequest =
                        SendTextMessageRequest.Create(user.ChatId, MessageText.Retailer.MainManu.SetUsername);
                    await _telegramBot.SendText(bot.Token, setUsernameRequest, cancellationToken);

                    await _retailerMainMenuStateActivator.Activate(bot, user, null, cancellationToken);
                    return;
                }

                await _addAdStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            case MenuText.Retailer.MainMenu.MyAds:
                await _myAdsStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            default:
                await _retailerMainMenuStateActivator.Activate(bot, user, null, cancellationToken);
                break;
        }
    }
}