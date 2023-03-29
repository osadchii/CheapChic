using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

public interface IAddAdPhotoStateHandler : IRetailerStateHandler
{
}

public class AddAdPhotoStateHandler : IAddAdPhotoStateHandler
{
    private readonly IAddAdPriceStateActivator _addAdPriceStateActivator;
    private readonly IAddAdPhotoStateActivator _addAdPhotoStateActivator;

    public AddAdPhotoStateHandler(IAddAdPriceStateActivator addAdPriceStateActivator,
        IAddAdPhotoStateActivator addAdPhotoStateActivator)
    {
        _addAdPriceStateActivator = addAdPriceStateActivator;
        _addAdPhotoStateActivator = addAdPhotoStateActivator;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        var state = stateData.FromJson<AddAdStateData>();
        switch (text)
        {
            case MenuText.Retailer.Common.Back:
                await _addAdPriceStateActivator.Activate(bot, user, state, cancellationToken);
                break;
            case MenuText.Retailer.AddAdPhotoMenu.ClearPhotos:
                state.Photos.Clear();
                await _addAdPhotoStateActivator.Activate(bot, user, state, cancellationToken);
                break;
            case MenuText.Retailer.AddAdPhotoMenu.Done:
                await _addAdPhotoStateActivator.Activate(bot, user, state, cancellationToken);
                break;
            default:
                await _addAdPhotoStateActivator.Activate(bot, user, state, cancellationToken);

                break;
        }
    }
}