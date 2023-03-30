using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Photo;
using Microsoft.Extensions.Options;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Common.Photo;

public class PhotoMessageHandler : IPhotoMessageHandler
{
    private readonly IOptions<ManagementBotOptions> _managementBotOptions;
    private readonly IRetailerPhotoMessageHandler _retailerPhotoMessageHandler;

    public PhotoMessageHandler(IOptions<ManagementBotOptions> managementBotOptions,
        IRetailerPhotoMessageHandler retailerPhotoMessageHandler)
    {
        _managementBotOptions = managementBotOptions;
        _retailerPhotoMessageHandler = retailerPhotoMessageHandler;
    }

    public Task HandleMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        var managementBotToken = _managementBotOptions.Value.Token;
        var isManagementBot = managementBotToken.Equals(token, StringComparison.InvariantCulture);

        if (isManagementBot)
        {
            return Task.CompletedTask;
        }

        return _retailerPhotoMessageHandler.HandleMessage(token, message, cancellationToken);
    }
}