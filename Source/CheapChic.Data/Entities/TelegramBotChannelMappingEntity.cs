using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.TelegramBotChannelMappingTable)]
public class TelegramBotChannelMappingEntity : BaseEntity
{
    [Required]
    [ForeignKey(nameof(TelegramBotEntity))]
    public Guid BotId { get; set; }

    public TelegramBotEntity Bot { get; set; }

    [Required]
    [ForeignKey(nameof(TelegramChannelEntity))]
    public Guid ChannelId { get; set; }

    public TelegramChannelEntity Channel { get; set; }
}