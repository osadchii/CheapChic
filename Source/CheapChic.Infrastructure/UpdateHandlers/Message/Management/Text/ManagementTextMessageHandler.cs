using CheapChic.Data;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text;

public class ManagementTextHandler : IManagementTextMessageHandler
{
    private readonly CheapChicContext _context;
    private readonly IUserService _userService;
    private readonly IManagementMainMenuStateActivator _managementMainMenuStateActivator;
    private readonly IManagementMainMenuStateHandler _managementMainMenuStateHandler;
    private readonly IManagementAddBotStateHandler _managementAddBotStateHandler;
    private readonly IManagementAddBotNameStateHandler _managementAddBotNameStateHandler;

    public ManagementTextHandler(CheapChicContext context, IUserService userService,
        IManagementMainMenuStateActivator managementMainMenuStateActivator, IManagementMainMenuStateHandler managementMainMenuStateHandler,
        IManagementAddBotStateHandler managementAddBotStateHandler, IManagementAddBotNameStateHandler managementAddBotNameStateHandler)
    {
        _context = context;
        _userService = userService;
        _managementMainMenuStateActivator = managementMainMenuStateActivator;
        _managementMainMenuStateHandler = managementMainMenuStateHandler;
        _managementAddBotStateHandler = managementAddBotStateHandler;
        _managementAddBotNameStateHandler = managementAddBotNameStateHandler;
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
            await _managementMainMenuStateActivator.Activate(token, user, null, cancellationToken);
            return;
        }

        IManagementStateHandler stateHandler = userState.State switch
        {
            State.ManagementMainMenu => _managementMainMenuStateHandler,
            State.ManagementAddBotToken => _managementAddBotStateHandler,
            State.ManagementAddBotName => _managementAddBotNameStateHandler,
            _ => null
        };

        if (stateHandler is not null)
        {
            await stateHandler.Handle(token, user, text, userState.Data, cancellationToken);
        }
    }
}