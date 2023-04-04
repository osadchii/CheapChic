using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

public interface IAddAdDescriptionStateHandler : IRetailerStateHandler
{
}

public class AddAdDescriptionStateHandler : IAddAdDescriptionStateHandler
{
    private const int DescriptionLength = 3072;

    private readonly IAddAdNameStateActivator _addAdNameStateActivator;

    private readonly IAddAdDescriptionStateActivator
        _addAdDescriptionStateActivator;

    private readonly IAddAdPriceStateActivator _addAdPriceStateActivator;

    public AddAdDescriptionStateHandler(
        IAddAdDescriptionStateActivator addAdDescriptionStateActivator,
        IAddAdNameStateActivator addAdNameStateActivator,
        IAddAdPriceStateActivator addAdPriceStateActivator)
    {
        _addAdDescriptionStateActivator = addAdDescriptionStateActivator;
        _addAdNameStateActivator = addAdNameStateActivator;
        _addAdPriceStateActivator = addAdPriceStateActivator;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        var state = stateData.FromJson<AddAdStateData>();
        switch (text)
        {
            case MenuText.Retailer.Common.Back:
                await _addAdNameStateActivator.Activate(bot, user, state, cancellationToken);
                break;
            default:
                var trimmedText = text.Trim();

                if (string.IsNullOrEmpty(trimmedText) || text.Length > DescriptionLength)
                {
                    await _addAdDescriptionStateActivator.Activate(bot, user, state, cancellationToken);
                }

                state.Description = trimmedText;
                await _addAdPriceStateActivator.Activate(bot, user, state, cancellationToken);

                break;
        }
    }
}