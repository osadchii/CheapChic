using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;

public interface IRetailerMainMenuStateHandler : IRetailerStateHandler
{
}

public class RetailerMainMenuStateHandler : IRetailerMainMenuStateHandler
{
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;
    private readonly IAddAdStateActivator _addAdStateActivator;

    public RetailerMainMenuStateHandler(IRetailerMainMenuStateActivator retailerMainMenuStateActivator,
        IAddAdStateActivator addAdStateActivator)
    {
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
        _addAdStateActivator = addAdStateActivator;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Retailer.MainMenu.AddAnnouncement:
                await _addAdStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            default:
                await _retailerMainMenuStateActivator.Activate(bot, user, null, cancellationToken);
                break;
        }
    }
}