using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.AddBot;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text.States.MainMenu;

public interface IManagementMainMenuStateHandler : IManagementStateHandler
{
}

public class ManagementMainMenuStateHandler : IManagementMainMenuStateHandler
{
    private readonly IManagementMainMenuStateActivator _managementMainMenuStateActivator;
    private readonly IManagementAddBotStateActivator _managementAddBotStateActivator;

    public ManagementMainMenuStateHandler(IManagementMainMenuStateActivator managementMainMenuStateActivator,
        IManagementAddBotStateActivator managementAddBotStateActivator)
    {
        _managementMainMenuStateActivator = managementMainMenuStateActivator;
        _managementAddBotStateActivator = managementAddBotStateActivator;
    }

    public async Task Handle(string token, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Management.MainMenu.AddBot:
                await _managementAddBotStateActivator.Activate(token, user, null, cancellationToken);
                break;
            default:
                await _managementMainMenuStateActivator.Activate(token, user, null, cancellationToken);
                break;
        }
    }
}