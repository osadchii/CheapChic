using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

public interface IMyBotsSettingsStateHandler : IManagementStateHandler
{
}

public class MyBotsSettingsStateHandler : IMyBotsSettingsStateHandler
{
    private readonly CheapChicContext _context;
    private readonly IMyBotsStateActivator _myBotsStateActivator;
    private readonly IMyBotsSettingsStateActivator _myBotsSettingsStateActivator;
    private readonly IMyBotsSettingsCurrencyStateActivator _myBotsSettingsCurrencyStateActivator;
    private readonly IMyBotsSettingsPublishDaysStateActivator _myBotsSettingsPublishDaysStateActivator;
    private readonly IMyBotsSettingsPublishEveryHoursStateActivator _myBotsSettingsPublishEveryHoursStateActivator;

    public MyBotsSettingsStateHandler(IMyBotsStateActivator myBotsStateActivator,
        IMyBotsSettingsStateActivator myBotsSettingsStateActivator, CheapChicContext context,
        IMyBotsSettingsCurrencyStateActivator myBotsSettingsCurrencyStateActivator,
        IMyBotsSettingsPublishDaysStateActivator myBotsSettingsPublishDaysStateActivator,
        IMyBotsSettingsPublishEveryHoursStateActivator myBotsSettingsPublishEveryHoursStateActivator)
    {
        _myBotsStateActivator = myBotsStateActivator;
        _myBotsSettingsStateActivator = myBotsSettingsStateActivator;
        _context = context;
        _myBotsSettingsCurrencyStateActivator = myBotsSettingsCurrencyStateActivator;
        _myBotsSettingsPublishDaysStateActivator = myBotsSettingsPublishDaysStateActivator;
        _myBotsSettingsPublishEveryHoursStateActivator = myBotsSettingsPublishEveryHoursStateActivator;
    }

    public async Task Handle(string token, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        var state = stateData.FromJson<MyBotStateData>();

        switch (text)
        {
            case MenuText.Management.Common.Back:
                await _myBotsStateActivator.Activate(token, user, null, cancellationToken);
                break;
            case MenuText.Management.MyBotsMenu.Disable:
            case MenuText.Management.MyBotsMenu.Enable:
                var telegramBot = await _context.TelegramBots
                    .Where(x => x.Id == state.BotId)
                    .FirstOrDefaultAsync(cancellationToken);

                telegramBot.Disabled = text == MenuText.Management.MyBotsMenu.Disable;
                await _context.SaveChangesAsync(cancellationToken);
                await _myBotsSettingsStateActivator.Activate(token, user, state, cancellationToken);
                break;
            case MenuText.Management.MyBotsMenu.Currency:
                await _myBotsSettingsCurrencyStateActivator.Activate(token, user, state, cancellationToken);
                break;
            case MenuText.Management.MyBotsMenu.PublishDays:
                await _myBotsSettingsPublishDaysStateActivator.Activate(token, user, state, cancellationToken);
                break;
            case MenuText.Management.MyBotsMenu.PublishEvery:
                await _myBotsSettingsPublishEveryHoursStateActivator.Activate(token, user, state, cancellationToken);
                break;
            default:
                await _myBotsSettingsStateActivator.Activate(token, user, state, cancellationToken);
                break;
        }
    }
}