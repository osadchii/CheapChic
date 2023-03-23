using CheapChic.Data;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text;

public class ManagementTextHandler : IManagementTextMessageHandler
{
    private readonly CheapChicContext _context;
    private readonly IUserService _userService;
    private readonly IMainMenuStateActivator _mainMenuStateActivator;
    private readonly IMainMenuStateHandler _mainMenuStateHandler;
    private readonly IAddBotStateHandler _addBotStateHandler;
    private readonly IAddBotNameStateHandler _addBotNameStateHandler;

    public ManagementTextHandler(CheapChicContext context, IUserService userService,
        IMainMenuStateActivator mainMenuStateActivator, IMainMenuStateHandler mainMenuStateHandler,
        IAddBotStateHandler addBotStateHandler, IAddBotNameStateHandler addBotNameStateHandler)
    {
        _context = context;
        _userService = userService;
        _mainMenuStateActivator = mainMenuStateActivator;
        _mainMenuStateHandler = mainMenuStateHandler;
        _addBotStateHandler = addBotStateHandler;
        _addBotNameStateHandler = addBotNameStateHandler;
    }

    public async Task HandleTextMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
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

        var userState = await _userService.GetUserState(user.Id, cancellationToken);

        if (userState is null)
        {
            await _mainMenuStateActivator.Activate(token, user, null, cancellationToken);
            return;
        }

        IStateHandler stateHandler = userState.State switch
        {
            State.ManagementMainMenu => _mainMenuStateHandler,
            State.ManagementAddBotToken => _addBotStateHandler,
            State.ManagementAddBotName => _addBotNameStateHandler,
            _ => null
        };

        if (stateHandler is not null)
        {
            await stateHandler.Handle(token, user, text, userState.Data, cancellationToken);
        }
    }
}