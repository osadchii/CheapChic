using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MyAds.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MyAds;

public interface IMyAdsSettingsStateHandler : IRetailerStateHandler
{
}

public class MyAdsSettingsStateHandler : IMyAdsSettingsStateHandler
{
    private readonly IMyAdsStateActivator _myAdsStateActivator;
    private readonly IMyAdsSettingsStateActivator _myAdsSettingsStateActivator;
    private readonly CheapChicContext _context;

    public MyAdsSettingsStateHandler(IMyAdsStateActivator myAdsStateActivator,
        IMyAdsSettingsStateActivator myAdsSettingsStateActivator, CheapChicContext context)
    {
        _myAdsStateActivator = myAdsStateActivator;
        _myAdsSettingsStateActivator = myAdsSettingsStateActivator;
        _context = context;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        var state = stateData.FromJson<MyAdsStateData>();
        switch (text)
        {
            case MenuText.Retailer.Common.Back:
                await _myAdsStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            case MenuText.Retailer.MyAdsMenu.Disable:
                var ad = await _context.Ads
                    .Where(x => x.Id == state.AdId)
                    .Where(x => x.BotId == bot.Id)
                    .Where(x => x.UserId == user.Id)
                    .Where(x => !x.Disable)
                    .FirstOrDefaultAsync(cancellationToken);

                if (ad is null)
                {
                    await _myAdsStateActivator.Activate(bot, user, null, cancellationToken);
                    return;
                }

                ad.Disable = true;
                await _context.SaveChangesAsync(cancellationToken);
                await _myAdsStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            default:
                await _myAdsSettingsStateActivator.Activate(bot, user, state, cancellationToken);
                break;
        }
    }
}