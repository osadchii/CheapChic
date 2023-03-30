using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Extensions;
using CheapChic.Infrastructure.Services.PhotoService;
using CheapChic.Infrastructure.Services.UserService;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.Services.PhotoAdService;

public class PhotoAdService : IPhotoAdService
{
    private readonly IPhotoService _photoService;
    private readonly ITelegramBot _telegramBot;
    private readonly IUserService _userService;
    private const int MaxPhotos = 4;

    public PhotoAdService(IPhotoService photoService, ITelegramBot telegramBot, IUserService userService)
    {
        _photoService = photoService;
        _telegramBot = telegramBot;
        _userService = userService;
    }

    public async Task HandlePhoto(string token, Guid telegramBotId, TelegramUserEntity user, string fileId,
        CancellationToken cancellationToken = default)
    {
        var chatId = user.ChatId;
        var userState = await _userService.GetUserState(user.Id, telegramBotId, cancellationToken);

        if (userState is null || userState.State != State.RetailerAddAdPhoto)
        {
            return;
        }

        var state = userState.Data.FromJson<AddAdStateData>();

        if (state.Photos.Count >= MaxPhotos)
        {
            var cantAddMorePhotos =
                SendTextMessageRequest.Create(chatId, MessageText.Retailer.AddAd.CantAddMoreThanPhotos(MaxPhotos));
            await _telegramBot.SendText(token, cantAddMorePhotos, cancellationToken);
            return;
        }

        var file = await _telegramBot.DownloadFile(token, fileId, cancellationToken);
        var photoId = await _photoService.GetPhotoId(file, cancellationToken);

        if (state.Photos.Contains(photoId))
        {
            var photoAlreadyInTheAd =
                SendMediaGroupRequest.Create(chatId, MessageText.Retailer.AddAd.PhotoAlreadyInTheAd, new[] { photoId });
            await _telegramBot.SendPhoto(token, photoAlreadyInTheAd, cancellationToken);
            return;
        }

        var request =
            SendMediaGroupRequest.Create(chatId, MessageText.Retailer.AddAd.PhotoAddedToTheAd, new[] { photoId });
        var result = await _telegramBot.SendPhoto(token, request, cancellationToken);

        if (result.Length == 0)
        {
            var cantAddRequest = SendTextMessageRequest.Create(chatId, MessageText.Retailer.AddAd.CantAddPhotoToTheAd);
            await _telegramBot.SendText(token, cantAddRequest, cancellationToken);
            return;
        }

        state.Photos.Add(photoId);
        await _userService.SetUserState(user.Id, telegramBotId, userState.State, state, cancellationToken);
    }
}