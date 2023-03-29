using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.TelegramMessageTable)]
public class TelegramMessageEntity : BaseEntity
{
    [Required] public int MessageId { get; set; }

    [ForeignKey(nameof(TelegramBotEntity))]
    public Guid? BotId { get; set; }

    public TelegramBotEntity Bot { get; set; }

    [ForeignKey(nameof(TelegramUserEntity))]
    public Guid? UserId { get; set; }

    public TelegramUserEntity User { get; set; }

    [ForeignKey(nameof(TelegramChannelEntity))]
    public Guid? ChannelId { get; set; }

    public TelegramChannelEntity Channel { get; set; }

    [Required] public TelegramMessageType Type { get; set; }

    [Required] public string Message { get; set; }

    public enum TelegramMessageType
    {
        Text,
        ReplyKeyboard,
        Photo
    }
}