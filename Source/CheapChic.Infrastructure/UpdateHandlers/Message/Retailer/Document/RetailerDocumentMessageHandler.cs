using CheapChic.Infrastructure.Services.PhotoAdService;
using CheapChic.Infrastructure.Services.TelegramBotService;
using CheapChic.Infrastructure.Services.UserService;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Document;

public class RetailerDocumentMessageHandler : IRetailerDocumentMessageHandler
{
    private const string ImageMimeType = "image";

    private readonly IUserService _userService;
    private readonly ITelegramBotService _telegramBotService;
    private readonly IPhotoAdService _photoAdService;

    public RetailerDocumentMessageHandler(IUserService userService, ITelegramBotService telegramBotService,
        IPhotoAdService photoAdService)
    {
        _userService = userService;
        _telegramBotService = telegramBotService;
        _photoAdService = photoAdService;
    }

    public async Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        var telegramBotEntity = await _telegramBotService.GetTelegramBot(token, cancellationToken);

        if (telegramBotEntity is null)
        {
            return;
        }

        var chatId = message.Chat.Id;
        var document = message.Document!;

        if (document.MimeType?.StartsWith(ImageMimeType) != true)
        {
            return;
        }

        var user = await _userService.GetUser(chatId, cancellationToken);

        if (user is null)
        {
            return;
        }

        await _photoAdService.HandlePhoto(token, telegramBotEntity.Id, user, document.FileId, cancellationToken);
    }
}