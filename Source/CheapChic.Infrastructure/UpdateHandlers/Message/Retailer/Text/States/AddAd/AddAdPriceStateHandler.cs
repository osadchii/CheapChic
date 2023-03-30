using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

public interface IAddAdPriceStateHandler : IRetailerStateHandler
{
}

public class AddAdPriceStateHandler : IAddAdPriceStateHandler
{
    private readonly IAddAdDescriptionStateActivator
        _addAdDescriptionStateActivator;

    private readonly IAddAdPriceStateActivator _addAdPriceStateActivator;
    private readonly IAddAdPhotoStateActivator _addAdPhotoStateActivator;

    public AddAdPriceStateHandler(
        IAddAdDescriptionStateActivator addAdDescriptionStateActivator,
        IAddAdPriceStateActivator addAdPriceStateActivator,
        IAddAdPhotoStateActivator addAdPhotoStateActivator)
    {
        _addAdDescriptionStateActivator = addAdDescriptionStateActivator;
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
                await _addAdDescriptionStateActivator.Activate(bot, user, state, cancellationToken);
                break;
            case MenuText.Retailer.AddAdPriceMenu.Negotiated:
                state.Price = null;
                await _addAdPhotoStateActivator.Activate(bot, user, state, cancellationToken);
                break;
            default:
                var formattedText = text.Replace('.', ',');
                if (!decimal.TryParse(formattedText, out var price))
                {
                    await _addAdPriceStateActivator.Activate(bot, user, state, cancellationToken);
                    return;
                }

                state.Price = price;
                await _addAdPhotoStateActivator.Activate(bot, user, state, cancellationToken);

                break;
        }
    }
}