using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

public interface IMyBotsStateHandler : IManagementStateHandler
{
}

public class MyBotsStateHandler : IMyBotsStateHandler
{
    private readonly CheapChicContext _context;
    private readonly ITelegramBot _telegramBot;
    private readonly IManagementMainMenuStateActivator _managementMainMenuStateActivator;
    private readonly IMyBotsStateActivator _myBotsStateActivator;
    private readonly IMyBotsSettingsStateActivator _myBotsSettingsStateActivator;

    public MyBotsStateHandler(IMyBotsStateActivator myBotsStateActivator,
        IManagementMainMenuStateActivator managementMainMenuStateActivator, CheapChicContext context,
        ITelegramBot telegramBot, IMyBotsSettingsStateActivator myBotsSettingsStateActivator)
    {
        _myBotsStateActivator = myBotsStateActivator;
        _managementMainMenuStateActivator = managementMainMenuStateActivator;
        _context = context;
        _telegramBot = telegramBot;
        _myBotsSettingsStateActivator = myBotsSettingsStateActivator;
    }

    public async Task Handle(string token, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Management.Common.Back:
                await _managementMainMenuStateActivator.Activate(token, user, null, cancellationToken);
                break;
            default:
                var telegramBotId = await _context.TelegramBots
                    .AsNoTracking()
                    .Include(x => x.Owner)
                    .Where(x => x.Name == text)
                    .Where(x => x.Owner.Id == user.Id)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (telegramBotId == default)
                {
                    var botNotFoundRequest = SendTextMessageRequest.Create(user.ChatId,
                        MessageText.Management.MyBots.BotWithNameNotFound(text));
                    await _telegramBot.SendText(token, botNotFoundRequest, cancellationToken);

                    await _myBotsStateActivator.Activate(token, user, null, cancellationToken);
                    return;
                }

                var state = new MyBotStateData
                {
                    BotId = telegramBotId
                };
                await _myBotsSettingsStateActivator.Activate(token, user, state, cancellationToken);
                break;
        }
    }
}