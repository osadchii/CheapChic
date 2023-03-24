using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot.Models;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;

public interface IManagementAddBotStateHandler : IManagementStateHandler
{
}

public class ManagementAddBotStateHandler : IManagementAddBotStateHandler
{
    private readonly IManagementMainMenuStateActivator _managementMainMenuStateActivator;
    private readonly IManagementAddBotStateActivator _managementAddBotStateActivator;
    private readonly IManagementAddBotNameStateActivator _managementAddBotNameStateActivator;
    private readonly ITelegramBot _telegramBot;
    private readonly CheapChicContext _context;

    public ManagementAddBotStateHandler(IManagementMainMenuStateActivator managementMainMenuStateActivator, ITelegramBot telegramBot,
        IManagementAddBotStateActivator managementAddBotStateActivator, CheapChicContext context,
        IManagementAddBotNameStateActivator managementAddBotNameStateActivator)
    {
        _managementMainMenuStateActivator = managementMainMenuStateActivator;
        _managementAddBotStateActivator = managementAddBotStateActivator;
        _managementAddBotNameStateActivator = managementAddBotNameStateActivator;
        _telegramBot = telegramBot;
        _context = context;
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
                var tokenValid = await _telegramBot.TestToken(text, cancellationToken);
                if (!tokenValid)
                {
                    var request =
                        SendTextMessageRequest.Create(user.ChatId, MessageText.Management.AddBot.InvalidToken);
                    await _telegramBot.SendText(token, request, cancellationToken);
                    await _managementAddBotStateActivator.Activate(token, user, null, cancellationToken);

                    return;
                }

                var tokenExists = await _context.TelegramBots
                    .AsNoTracking()
                    .Where(x => x.Token == text)
                    .AnyAsync(cancellationToken);

                if (tokenExists)
                {
                    var request =
                        SendTextMessageRequest.Create(user.ChatId, MessageText.Management.AddBot.TokenAlreadyExists);
                    await _telegramBot.SendText(token, request, cancellationToken);
                    await _managementAddBotStateActivator.Activate(token, user, null, cancellationToken);

                    return;
                }

                var state = new AddBotStateData
                {
                    Token = text
                };

                await _managementAddBotNameStateActivator.Activate(token, user, state, cancellationToken);
                break;
        }
    }
}