using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Services.TelegramBotService;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.MainMenu;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text;

public class RetailerTextMessageHandler : IRetailerTextMessageHandler
{
    private readonly IUserService _userService;
    private readonly IRetailerMainMenuStateActivator _retailerMainMenuStateActivator;
    private readonly IRetailerMainMenuStateHandler _retailerMainMenuStateHandler;
    private readonly IAddAdStateHandler _addAdStateHandler;
    private readonly IAddAdNameStateHandler _addAdNameStateHandler;
    private readonly IAddAdDescriptionStateHandler _addAdDescriptionStateHandler;
    private readonly IAddAdPriceStateHandler _addAdPriceStateHandler;
    private readonly IAddAdPhotoStateHandler _addAdPhotoStateHandler;
    private readonly IAddAdConfirmationStateHandler _addAdConfirmationStateHandler;
    private readonly ITelegramBotService _telegramBotService;

    public RetailerTextMessageHandler(IUserService userService,
        IRetailerMainMenuStateActivator retailerMainMenuStateActivator,
        IRetailerMainMenuStateHandler retailerMainMenuStateHandler,
        IAddAdStateHandler addAdStateHandler,
        IAddAdNameStateHandler addAdNameStateHandler,
        IAddAdDescriptionStateHandler addAdDescriptionStateHandler,
        IAddAdPriceStateHandler addAdPriceStateHandler,
        IAddAdPhotoStateHandler addAdPhotoStateHandler, ITelegramBotService telegramBotService,
        IAddAdConfirmationStateHandler addAdConfirmationStateHandler)
    {
        _userService = userService;
        _retailerMainMenuStateActivator = retailerMainMenuStateActivator;
        _retailerMainMenuStateHandler = retailerMainMenuStateHandler;
        _addAdStateHandler = addAdStateHandler;
        _addAdNameStateHandler = addAdNameStateHandler;
        _addAdDescriptionStateHandler = addAdDescriptionStateHandler;
        _addAdPriceStateHandler = addAdPriceStateHandler;
        _addAdPhotoStateHandler = addAdPhotoStateHandler;
        _telegramBotService = telegramBotService;
        _addAdConfirmationStateHandler = addAdConfirmationStateHandler;
    }

    public async Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        var telegramBot = await _telegramBotService.GetTelegramBot(token, cancellationToken);

        if (telegramBot is null)
        {
            return;
        }

        var chatId = message.Chat.Id;
        var text = message.Text!;

        var user = await _userService.GetUser(chatId, cancellationToken);

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
            State.RetailerAddAdAction => _addAdStateHandler,
            State.RetailerAddAdName => _addAdNameStateHandler,
            State.RetailerAddAdDescription => _addAdDescriptionStateHandler,
            State.RetailerAddAdPrice => _addAdPriceStateHandler,
            State.RetailerAddAdPhoto => _addAdPhotoStateHandler,
            State.RetailerAddAdConfirmation => _addAdConfirmationStateHandler,
            _ => null
        };

        if (stateHandler is not null)
        {
            await stateHandler.Handle(telegramBot, user, text, userState.Data, cancellationToken);
        }
    }
}