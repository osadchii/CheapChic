using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MyAds;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;

public interface IRetailerMainMenuStateHandler : IRetailerStateHandler
{
}

public class RetailerMainMenuStateHandler : IRetailerMainMenuStateHandler
{
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;
    private readonly IAddAdStateActivator _addAdStateActivator;
    private readonly IMyAdsStateActivator _myAdsStateActivator;

    public RetailerMainMenuStateHandler(IRetailerMainMenuStateActivator retailerMainMenuStateActivator,
        IAddAdStateActivator addAdStateActivator, IMyAdsStateActivator myAdsStateActivator)
    {
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
        _addAdStateActivator = addAdStateActivator;
        _myAdsStateActivator = myAdsStateActivator;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Retailer.MainMenu.AddAd:
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