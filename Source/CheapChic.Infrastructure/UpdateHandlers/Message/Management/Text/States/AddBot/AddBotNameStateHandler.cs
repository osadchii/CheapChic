using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot.Models;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;

public interface IAddBotNameStateHandler : IManagementStateHandler
{
}

public class AddBotNameStateHandler : IAddBotNameStateHandler
{
    private readonly IManagementMainMenuStateActivator _managementMainMenuStateActivator;
    private readonly IAddBotStateActivator _addBotStateActivator;
    private readonly IAddBotNameStateActivator _addBotNameStateActivator;
    private readonly ITelegramBot _telegramBot;
    private readonly CheapChicContext _context;

    public AddBotNameStateHandler(IManagementMainMenuStateActivator managementMainMenuStateActivator, ITelegramBot telegramBot,
        IAddBotStateActivator addBotStateActivator, CheapChicContext context,
        IAddBotNameStateActivator addBotNameStateActivator)
    {
        _managementMainMenuStateActivator = managementMainMenuStateActivator;
        _addBotStateActivator = addBotStateActivator;
        _addBotNameStateActivator = addBotNameStateActivator;
        _telegramBot = telegramBot;
        _context = context;
    }

    public async Task Handle(string token, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        var state = stateData.FromJson<AddBotStateData>();
        var trimmedText = text.Trim();

        switch (text)
        {
            case MenuText.Management.Common.Back:
                await _addBotStateActivator.Activate(token, user, null, cancellationToken);
                break;
            default:
                if (trimmedText.Length > 16)
                {
                    var request =
                        SendTextMessageRequest.Create(user.ChatId, MessageText.Management.AddBot.NameIsTooLong);
                    await _telegramBot.SendText(token, request, cancellationToken);
                    await _addBotNameStateActivator.Activate(token, user, state, cancellationToken);
                    
                    return;
                }
                
                var nameExists = await _context.TelegramBots
                    .AsNoTracking()
                    .Where(x => x.OwnerId == user.Id)
                    // ReSharper disable once SpecifyStringComparison
                    .Where(x => x.Name.ToLower() == trimmedText.ToLower())
                    .AnyAsync(cancellationToken);

                if (nameExists)
                {
                    var request =
                        SendTextMessageRequest.Create(user.ChatId, MessageText.Management.AddBot.NameAlreadyExists);
                    await _telegramBot.SendText(token, request, cancellationToken);
                    await _addBotNameStateActivator.Activate(token, user, state, cancellationToken);
                    
                    return;
                }

                var telegramBot = new TelegramBotEntity
                {
                    Name = trimmedText,
                    OwnerId = user.Id,
                    Token = state.Token
                };

                await _context.AddAsync(telegramBot, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await _telegramBot.SetWebhook(state.Token, cancellationToken);
                
                var successRequest =
                    SendTextMessageRequest.Create(user.ChatId, MessageText.Management.AddBot.SuccessfulAdded);
                await _telegramBot.SendText(token, successRequest, cancellationToken);

                await _managementMainMenuStateActivator.Activate(token, user, null, cancellationToken);

                break;
        }
    }
}