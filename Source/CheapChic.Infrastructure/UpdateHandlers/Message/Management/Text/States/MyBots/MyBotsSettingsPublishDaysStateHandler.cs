using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

public interface IMyBotsSettingsPublishDaysStateHandler : IManagementStateHandler
{
}

public class MyBotsSettingsPublishDaysStateHandler : IMyBotsSettingsPublishDaysStateHandler
{
    private readonly IMyBotsSettingsStateActivator _myBotsSettingsStateActivator;
    private readonly IMyBotsSettingsPublishDaysStateActivator _myBotsSettingsPublishDaysStateActivator;
    private readonly CheapChicContext _context;

    public MyBotsSettingsPublishDaysStateHandler(IMyBotsSettingsStateActivator myBotsSettingsStateActivator,
        CheapChicContext context, IMyBotsSettingsPublishDaysStateActivator myBotsSettingsPublishDaysStateActivator)
    {
        _myBotsSettingsStateActivator = myBotsSettingsStateActivator;
        _context = context;
        _myBotsSettingsPublishDaysStateActivator = myBotsSettingsPublishDaysStateActivator;
    }

    public async Task Handle(string token, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        var state = stateData.FromJson<MyBotStateData>();

        switch (text)
        {
            case MenuText.Management.Common.Back:
                await _myBotsSettingsStateActivator.Activate(token, user, state, cancellationToken);
                break;
            default:
                if (!int.TryParse(text, out var days) || days is > 28 or < 0)
                {
                    await _myBotsSettingsPublishDaysStateActivator.Activate(token, user, state, cancellationToken);
                    return;
                }

                var telegramBot = await _context.TelegramBots
                    .Where(x => x.Id == state.BotId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (telegramBot is null)
                {
                    await _myBotsSettingsPublishDaysStateActivator.Activate(token, user, state, cancellationToken);
                    return;
                }

                telegramBot.PublishForDays = days;
                await _context.SaveChangesAsync(cancellationToken);

                await _myBotsSettingsStateActivator.Activate(token, user, state, cancellationToken);
                break;
        }
    }
}