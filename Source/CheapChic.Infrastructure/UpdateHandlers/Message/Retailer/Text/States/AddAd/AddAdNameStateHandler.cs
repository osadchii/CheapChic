using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

public interface IAddAdNameStateHandler : IRetailerStateHandler
{
}

public class AddAdNameStateHandler : IAddAdNameStateHandler
{
    private const int NameLength = 128;

    private readonly IAddAdStateActivator _addAdStateActivator;

    private readonly IAddAdDescriptionStateActivator
        _addAdDescriptionStateActivator;

    public AddAdNameStateHandler(
        IAddAdStateActivator addAdStateActivator,
        IAddAdDescriptionStateActivator addAdDescriptionStateActivator)
    {
        _addAdStateActivator = addAdStateActivator;
        _addAdDescriptionStateActivator = addAdDescriptionStateActivator;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        var state = stateData.FromJson<AddAdStateData>();
        switch (text)
        {
            case MenuText.Retailer.Common.Back:
                await _addAdStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            default:
                var trimmedText = text.Trim();

                if (string.IsNullOrEmpty(trimmedText) || text.Length > NameLength)
                {
                    await _addAdStateActivator.Activate(bot, user, null, cancellationToken);
                }

                state.Name = trimmedText;
                await _addAdDescriptionStateActivator.Activate(bot, user, state, cancellationToken);

                break;
        }
    }
}