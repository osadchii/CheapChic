using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.UpdateHandlers.Message.Retailer.Text.States.AddAd.Models;

namespace CheapChic.Infrastructure.Services.AdMessageBuilder;

public interface IAdMessageBuilder
{
    SendRequest BuildByState(AddAdStateData stateData, long chatId, string username, string currency);
    SendRequest BuildByEntity(AdEntity ad, List<AdPhotoEntity> photos, long chatId, string username, string currency);
}