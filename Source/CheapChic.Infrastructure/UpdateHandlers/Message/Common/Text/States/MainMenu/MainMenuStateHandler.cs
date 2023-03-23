using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text.States.AddBot;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text.States.MainMenu;

public interface IMainMenuStateHandler : IStateHandler
{
}

public class MainMenuStateHandler : IMainMenuStateHandler
{
    private readonly IMainMenuStateActivator _mainMenuStateActivator;
    private readonly IAddBotStateActivator _addBotStateActivator;

    public MainMenuStateHandler(IMainMenuStateActivator mainMenuStateActivator,
        IAddBotStateActivator addBotStateActivator)
    {
        _mainMenuStateActivator = mainMenuStateActivator;
        _addBotStateActivator = addBotStateActivator;
    }

    public async Task Handle(string token, TelegramUserEntity user, string text, string stateData,
        CancellationToken cancellationToken)
    {
        switch (text)
        {
            case MenuText.Management.MainMenu.AddBot:
                await _addBotStateActivator.Activate(token, user, null, cancellationToken);
                break;
            default:
                await _mainMenuStateActivator.Activate(token, user, null, cancellationToken);
                break;
        }
    }
}