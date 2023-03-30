using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

public interface IAddAdStateHandler : IRetailerStateHandler
{
}

public class AddAdStateHandler : IAddAdStateHandler
{
    private readonly IAddAdStateActivator _addAdStateActivator;
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;
    private readonly IAddAdNameStateActivator _addAdNameStateActivator;

    public AddAdStateHandler(
        IAddAdStateActivator addAdStateActivator,
        IRetailerMainMenuStateActivator retailerMainMenuStateActivator,
        IAddAdNameStateActivator addAdNameStateActivator)
    {
        _addAdStateActivator = addAdStateActivator;
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
        _addAdNameStateActivator = addAdNameStateActivator;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Retailer.AddAdMenu.Buy:
            case MenuText.Retailer.AddAdMenu.Sell:
            case MenuText.Retailer.AddAdMenu.OfferAService:
            case MenuText.Retailer.AddAdMenu.LookingForAService:
                var state = new AddAdStateData
                {
                    Action = text
                };
                await _addAdNameStateActivator.Activate(bot, user, state, cancellationToken);
                break;
            case MenuText.Retailer.Common.Back:
                await _retailerMainMenuStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            default:
                await _addAdStateActivator.Activate(bot, user, null, cancellationToken);
                break;
        }
    }
}