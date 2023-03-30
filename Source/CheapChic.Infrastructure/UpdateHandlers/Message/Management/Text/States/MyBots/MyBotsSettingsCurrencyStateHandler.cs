using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots.Models;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

public interface IMyBotsSettingsCurrencyStateHandler : IManagementStateHandler
{
}

public class MyBotsSettingsCurrencyStateHandler : IMyBotsSettingsCurrencyStateHandler
{
    private readonly IMyBotsSettingsStateActivator _myBotsSettingsStateActivator;
    private readonly IMyBotsSettingsCurrencyStateActivator _myBotsSettingsCurrencyStateActivator;
    private readonly CheapChicContext _context;

    public MyBotsSettingsCurrencyStateHandler(IMyBotsSettingsStateActivator myBotsSettingsStateActivator,
        CheapChicContext context, IMyBotsSettingsCurrencyStateActivator myBotsSettingsCurrencyStateActivator)
    {
        _myBotsSettingsStateActivator = myBotsSettingsStateActivator;
        _context = context;
        _myBotsSettingsCurrencyStateActivator = myBotsSettingsCurrencyStateActivator;
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
                var trimmedText = text.Trim();
                if (trimmedText.Length > 5)
                {
                    await _myBotsSettingsCurrencyStateActivator.Activate(token, user, state, cancellationToken);
                    return;
                }
                
                var telegramBot = await _context.TelegramBots
                    .Where(x => x.Id == state.BotId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (telegramBot is null)
                {
                    await _myBotsSettingsCurrencyStateActivator.Activate(token, user, state, cancellationToken);
                    return;
                }

                telegramBot.Currency = text;
                await _context.SaveChangesAsync(cancellationToken);

                await _myBotsSettingsStateActivator.Activate(token, user, state, cancellationToken);
                break;
        }
    }
}