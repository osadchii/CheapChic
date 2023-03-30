using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;
using Microsoft.EntityFrameworkCore;

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

    private readonly IAddAdNameStateActivator _addAdNameStateActivator;

    private readonly CheapChicContext _context;
    private readonly ITelegramBot _telegramBot;

    public AddAdNameStateHandler(
        IAddAdStateActivator addAdStateActivator,
        IAddAdDescriptionStateActivator addAdDescriptionStateActivator, CheapChicContext context,
        IAddAdNameStateActivator addAdNameStateActivator, ITelegramBot telegramBot)
    {
        _addAdStateActivator = addAdStateActivator;
        _addAdDescriptionStateActivator = addAdDescriptionStateActivator;
        _context = context;
        _addAdNameStateActivator = addAdNameStateActivator;
        _telegramBot = telegramBot;
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

                var isNotUnique = await _context.Ads
                    .AsNoTracking()
                    .Where(x => !x.Disable)
                    .Where(x => x.BotId == bot.Id)
                    .Where(x => x.UserId == user.Id)
                    // ReSharper disable once SpecifyStringComparison
                    .Where(x => x.Name.ToLower() == trimmedText.ToLower())
                    .Select(x => x.Id)
                    .AnyAsync(cancellationToken);

                if (isNotUnique)
                {
                    var notUniqueRequest =
                        SendTextMessageRequest.Create(user.ChatId, MessageText.Retailer.AddAd.NameIsNotUnique);

                    await _telegramBot.SendText(bot.Token, notUniqueRequest, cancellationToken);
                    await _addAdNameStateActivator.Activate(bot, user, state, cancellationToken);
                    return;
                }

                state.Name = trimmedText;
                await _addAdDescriptionStateActivator.Activate(bot, user, state, cancellationToken);

                break;
        }
    }
}