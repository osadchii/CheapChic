using System.Globalization;
using System.Text;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.Services.AdMessageBuilder;

public class AdMessageBuilder : IAdMessageBuilder
{
    public SendRequest BuildByState(AddAdStateData stateData, long chatId, string username, string currency)
    {
        return Build(chatId, stateData.Action, stateData.Name, stateData.Description, username, stateData.Price,
            currency,
            stateData.Photos.ToArray());
    }

    public SendRequest BuildByEntity(AdEntity ad, List<AdPhotoEntity> photos, long chatId, string username,
        string currency)
    {
        return Build(chatId, ad.Action, ad.Name, ad.Description, username, ad.Price, currency,
            photos.Select(x => x.PhotoId).ToArray());
    }

    private static SendRequest Build(long chatId, string action, string name, string description, string username,
        decimal? price, string currency, Guid[] photos)
    {
        const string deductedPrice = "Договорная";

        var priceText = price.HasValue ? 
            $"Цена: <b>{price.Value.ToString(CultureInfo.InvariantCulture)} {currency}</b>" : 
            deductedPrice;

        var sb = new StringBuilder($"<b>{name}</b> от @{username}");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine($"<i>{description}</i>");
        sb.AppendLine();
        sb.AppendLine(priceText);
        sb.AppendLine();
        sb.AppendLine($"#{action.Replace(" ", "")}");

        SendRequest result;
        if (photos.Length > 0)
        {
            result = SendMediaGroupRequest.Create(chatId, sb.ToString(), photos);
        }
        else
        {
            result = SendTextMessageRequest.Create(chatId, sb.ToString());
        }

        return result;
    }
}