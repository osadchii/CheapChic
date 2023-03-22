using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text;
using Microsoft.Extensions.Options;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Common.Text;

public class TextMessageHandler : ITextMessageHandler
{
    private readonly IOptions<ManagementBotOptions> _managementBotOptions;
    private readonly IRetailerTextMessageHandler _retailerTextMessageHandler;
    private readonly IManagementTextMessageHandler _managementTextMessageHandler;

    public TextMessageHandler(IOptions<ManagementBotOptions> managementBotOptions, IRetailerTextMessageHandler retailerTextMessageHandler,
        IManagementTextMessageHandler managementTextMessageHandler)
    {
        _managementBotOptions = managementBotOptions;
        _retailerTextMessageHandler = retailerTextMessageHandler;
        _managementTextMessageHandler = managementTextMessageHandler;
    }


    public Task HandleTextMessage(string token, Telegram.Bot.Types.Message message,
        CancellationToken cancellationToken = default)
    {
        var managementBotToken = _managementBotOptions.Value.Token;
        var isManagementBot = managementBotToken.Equals(token, StringComparison.InvariantCulture);

        ITextMessageHandler handler = isManagementBot ? _managementTextMessageHandler : _retailerTextMessageHandler;

        return handler.HandleTextMessage(token, message, cancellationToken);
    }
}