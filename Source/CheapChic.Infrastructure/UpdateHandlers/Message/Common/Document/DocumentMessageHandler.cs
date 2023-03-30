using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Document;
using Microsoft.Extensions.Options;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Common.Document;

public class DocumentMessageHandler : IDocumentMessageHandler
{
    private readonly IOptions<ManagementBotOptions> _managementBotOptions;
    private readonly IRetailerDocumentMessageHandler _retailerDocumentMessageHandler;

    public DocumentMessageHandler(IOptions<ManagementBotOptions> managementBotOptions, IRetailerDocumentMessageHandler retailerDocumentMessageHandler)
    {
        _managementBotOptions = managementBotOptions;
        _retailerDocumentMessageHandler = retailerDocumentMessageHandler;
    }

    public Task HandleMessage(string token, Telegram.Bot.Types.Message message, CancellationToken cancellationToken = default)
    {
        var managementBotToken = _managementBotOptions.Value.Token;
        var isManagementBot = managementBotToken.Equals(token, StringComparison.InvariantCulture);

        if (isManagementBot)
        {
            return Task.CompletedTask;
        }

        return _retailerDocumentMessageHandler.HandleMessage(token, message, cancellationToken);
    }
}