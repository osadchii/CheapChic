using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

public interface IMyBotsSettingsPublishEveryHoursStateHandler : IManagementStateHandler
{
}

public class MyBotsSettingsPublishEveryHoursStateHandler : IMyBotsSettingsPublishEveryHoursStateHandler
{
    private readonly IMyBotsSettingsStateActivator _myBotsSettingsStateActivator;
    private readonly IMyBotsSettingsPublishEveryHoursStateActivator _myBotsSettingsPublishEveryHoursStateActivator;
    private readonly CheapChicContext _context;

    public MyBotsSettingsPublishEveryHoursStateHandler(IMyBotsSettingsStateActivator myBotsSettingsStateActivator,
        CheapChicContext context, IMyBotsSettingsPublishEveryHoursStateActivator myBotsSettingsPublishEveryHoursStateActivator)
    {
        _myBotsSettingsStateActivator = myBotsSettingsStateActivator;
        _context = context;
        _myBotsSettingsPublishEveryHoursStateActivator = myBotsSettingsPublishEveryHoursStateActivator;
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
                if (!int.TryParse(text, out var hours) || hours < 1)
                {
                    await _myBotsSettingsPublishEveryHoursStateActivator.Activate(token, user, state, cancellationToken);
                    return;
                }

                var telegramBot = await _context.TelegramBots
                    .Where(x => x.Id == state.BotId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (telegramBot is null)
                {
                    await _myBotsSettingsPublishEveryHoursStateActivator.Activate(token, user, state, cancellationToken);
                    return;
                }

                telegramBot.PublishEveryHours = hours;
                await _context.SaveChangesAsync(cancellationToken);

                await _myBotsSettingsStateActivator.Activate(token, user, state, cancellationToken);
                break;
        }
    }
}