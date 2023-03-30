using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

public interface IMyBotsStateActivator : IManagementStateActivator
{
    
}

public class MyBotsStateActivator : IMyBotsStateActivator
{
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;
    private readonly CheapChicContext _context;

    public MyBotsStateActivator(ITelegramBot telegramBot, IUserService userService, CheapChicContext context)
    {
        _telegramBot = telegramBot;
        _userService = userService;
        _context = context;
    }

    public async Task Activate(string token, TelegramUserEntity user, object stateData, CancellationToken cancellationToken = default)
    {
        var request = SendReplyKeyboardRequest.Create(user.ChatId, MessageText.Management.Common.SelectAMenuItem,
            await ConstantMenu.Management.MyBotsMenu(_context, user.Id, cancellationToken));

        await _telegramBot.SendReplyKeyboard(token, request, cancellationToken);
        await _userService.SetUserState(user.Id, State.ManagementMyBots, stateData, cancellationToken);
    }
}