using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MyAds.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MyAds;

public interface IMyAdsStateHandler : IRetailerStateHandler
{
}

public class MyAdsStateHandler : IMyAdsStateHandler
{
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;
    private readonly IMyAdsSettingsStateActivator _myAdsSettingsStateActivator;
    private readonly IMyAdsStateActivator _myAdsStateActivator;
    private readonly CheapChicContext _context;

    public MyAdsStateHandler(IRetailerMainMenuStateActivator retailerMainMenuStateActivator,
        IMyAdsStateActivator myAdsStateActivator, CheapChicContext context,
        IMyAdsSettingsStateActivator myAdsSettingsStateActivator)
    {
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
        _myAdsStateActivator = myAdsStateActivator;
        _context = context;
        _myAdsSettingsStateActivator = myAdsSettingsStateActivator;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Retailer.Common.Back:
                await _retailerMainMenuStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            default:
                var trimmedText = text.Trim().ToLower();

                var adId = await _context.Ads
                    .AsNoTracking()
                    .Where(x => x.BotId == bot.Id)
                    .Where(x => x.UserId == user.Id)
                    .Where(x => !x.Disable)
                    .Where(x => x.Name.ToLower() == trimmedText)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (adId == default)
                {
                    await _myAdsStateActivator.Activate(bot, user, null, cancellationToken);
                    return;
                }

                var state = new MyAdsStateData
                {
                    AdId = adId
                };

                await _myAdsSettingsStateActivator.Activate(bot, user, state, cancellationToken);
                break;
        }
    }
}