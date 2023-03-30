using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MyBots;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;

public interface IManagementMainMenuStateHandler : IManagementStateHandler
{
}

public class ManagementMainMenuStateHandler : IManagementMainMenuStateHandler
{
    private readonly IManagementMainMenuStateActivator _managementMainMenuStateActivator;
    private readonly IAddBotStateActivator _addBotStateActivator;
    private readonly IMyBotsStateActivator _myBotsStateActivator;

    public ManagementMainMenuStateHandler(IManagementMainMenuStateActivator managementMainMenuStateActivator,
        IAddBotStateActivator addBotStateActivator,
        IMyBotsStateActivator myBotsStateActivator)
    {
        _managementMainMenuStateActivator = managementMainMenuStateActivator;
        _addBotStateActivator = addBotStateActivator;
        _myBotsStateActivator = myBotsStateActivator;
    }

    public async Task Handle(string token, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Management.MainMenu.AddBot:
                await _addBotStateActivator.Activate(token, user, null, cancellationToken);
                break;
            case MenuText.Management.MainMenu.MyBots:
                await _myBotsStateActivator.Activate(token, user, null, cancellationToken);
                break;
            default:
                await _managementMainMenuStateActivator.Activate(token, user, null, cancellationToken);
                break;
        }
    }
}