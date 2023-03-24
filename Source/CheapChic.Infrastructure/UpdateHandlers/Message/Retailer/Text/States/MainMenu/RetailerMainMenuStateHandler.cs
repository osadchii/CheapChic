using CheapChic.Data.Entities;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;

public interface IRetailerMainMenuStateHandler : IRetailerStateHandler
{
    
}

public class RetailerMainMenuStateHandler : IRetailerMainMenuStateHandler
{
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;

    public RetailerMainMenuStateHandler(IRetailerMainMenuStateActivator retailerMainMenuStateActivator)
    {
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            default:
                await _retailerMainMenuStateActivator.Activate(bot, user, null, cancellationToken);
                break;
        }
    }
}