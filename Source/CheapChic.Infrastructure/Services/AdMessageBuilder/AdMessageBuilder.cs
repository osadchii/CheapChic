using System.Globalization;
using System.Text;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.Services.AdMessageBuilder;

public class AdMessageBuilder : IAdMessageBuilder
{
    public SendRequest BuildByState(AddAdStateData stateData, long chatId, string username)
    {
        var price = stateData.Price.HasValue
            ? stateData.Price.Value.ToString(CultureInfo.InvariantCulture)
            : "Договорная";
        var sb = new StringBuilder($"<b>{stateData.Name}</b> от @{username}");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine($"<i>{stateData.Description}</i>");
        sb.AppendLine();
        sb.AppendLine($"Цена: <b>{price}</b>");
        sb.AppendLine($"#{stateData.Action.Replace(" ", "")}");

        SendRequest result;
        if (stateData.Photos.Count > 0)
        {
            result = SendMediaGroupRequest.Create(chatId, sb.ToString(), stateData.Photos.ToArray());
        }
        else
        {
            result = SendTextMessageRequest.Create(chatId, sb.ToString());
        }

        return result;
    }

    public SendRequest BuildByEntity(AdEntity ad, List<AdPhotoEntity> photos, long chatId, string username)
    {
        var price = ad.Price.HasValue ? ad.Price.Value.ToString(CultureInfo.InvariantCulture) : "Договорная";
        var sb = new StringBuilder($"<b>{ad.Name}</b> от @{username}");
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine($"<i>{ad.Description}</i>");
        sb.AppendLine();
        sb.AppendLine($"Цена: <b>{price}</b>");
        sb.AppendLine($"#{ad.Action.Replace(" ", "")}");

        SendRequest result;
        if (photos.Count > 0)
        {
            result = SendMediaGroupRequest.Create(chatId, sb.ToString(), photos.Select(x => x.PhotoId).ToArray());
        }
        else
        {
            result = SendTextMessageRequest.Create(chatId, sb.ToString());
        }

        return result;
    }
}