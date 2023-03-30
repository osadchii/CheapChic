using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;

public interface IAddAdConfirmationStateHandler : IRetailerStateHandler
{
}

public class AddAdConfirmationStateHandler : IAddAdConfirmationStateHandler
{
    private readonly IAddAdPhotoStateActivator _addAdPhotoStateActivator;
    private readonly IAddAdConfirmationStateActivator _addAdConfirmationStateActivator;
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;
    private readonly CheapChicContext _context;
    private readonly ITelegramBot _telegramBot;

    public AddAdConfirmationStateHandler(IAddAdPhotoStateActivator addAdPhotoStateActivator,
        IAddAdConfirmationStateActivator addAdConfirmationStateActivator, CheapChicContext context,
        IRetailerMainMenuStateActivator retailerMainMenuStateActivator, ITelegramBot telegramBot)
    {
        _addAdPhotoStateActivator = addAdPhotoStateActivator;
        _addAdConfirmationStateActivator = addAdConfirmationStateActivator;
        _context = context;
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
        _telegramBot = telegramBot;
    }

    public async Task Handle(TelegramBotEntity bot, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        var state = stateData.FromJson<AddAdStateData>();
        switch (text)
        {
            case MenuText.Retailer.Common.Back:
                await _addAdPhotoStateActivator.Activate(bot, user, state, cancellationToken);
                break;
            case MenuText.Retailer.AddAdConfirmationMenu.Publish:
                var ad = new AdEntity
                {
                    Action = state.Action,
                    Date = DateTime.UtcNow,
                    Name = state.Name,
                    Description = state.Description,
                    BotId = bot.Id,
                    UserId = user.Id,
                    Price = state.Price
                };
                await _context.AddAsync(ad, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var photos = state.Photos
                    .Select(x => new AdPhotoEntity
                    {
                        AdId = ad.Id,
                        PhotoId = x
                    });

                await _context.AddRangeAsync(photos, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var publishedRequest =
                    SendTextMessageRequest.Create(user.ChatId, MessageText.Retailer.AddAd.AdPublished);
                await _telegramBot.SendText(bot.Token, publishedRequest, cancellationToken);

                await _retailerMainMenuStateActivator.Activate(bot, user, null, cancellationToken);
                break;
            default:
                await _addAdConfirmationStateActivator.Activate(bot, user, state, cancellationToken);
                break;
        }
    }
}