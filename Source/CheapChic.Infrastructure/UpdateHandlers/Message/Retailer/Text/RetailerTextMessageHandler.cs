using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text;

public class RetailerTextMessageHandler : IRetailerTextMessageHandler
{
    private readonly CheapChicContext _context;
    private readonly IUserService _userService;
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;
    private readonly IRetailerMainMenuStateHandler _retailerMainMenuStateHandler;

    public RetailerTextMessageHandler(CheapChicContext context, IUserService userService,
        IRetailerMainMenuStateActivator retailerMainMenuStateActivator, IRetailerMainMenuStateHandler retailerMainMenuStateHandler)
    {
        _context = context;
        _userService = userService;
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
        _retailerMainMenuStateHandler = retailerMainMenuStateHandler;
    }

    public async Task HandleTextMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        var telegramBot = await GetTelegramBot(token, cancellationToken);

        if (telegramBot is null)
        {
            return;
        }

        var chatId = message.Chat.Id;
        var text = message.Text!;

        var user = await _context.TelegramUsers
            .AsNoTracking()
            .Where(x => !x.Disabled)
            .Where(x => x.ChatId == chatId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return;
        }

        var userState = await _userService.GetUserState(user.Id, telegramBot.Id, cancellationToken);

        if (userState is null)
        {
            await _retailerMainMenuStateActivator.Activate(telegramBot, user, null, cancellationToken);
            return;
        }

        IRetailerStateHandler stateHandler = userState.State switch
        {
            State.RetailerMainMenu => _retailerMainMenuStateHandler,
            _ => null
        };

        if (stateHandler is not null)
        {
            await stateHandler.Handle(telegramBot, user, text, userState.Data, cancellationToken);
        }
    }

    private Task<TelegramBotEntity> GetTelegramBot(string token, CancellationToken cancellationToken)
    {
        return _context.TelegramBots
            .AsNoTracking()
            .Where(x => x.Disabled == false)
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }
}