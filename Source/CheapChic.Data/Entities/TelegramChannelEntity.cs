using System.ComponentModel.DataAnnotations;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.TelegramChannelTable)]
public class TelegramChannelEntity : BaseEntity
{
    [Required]
    public long ChatId { get; set; }

    [Required]
    [MaxLength(DatabaseLimit.TelegramChannelName)]
    public string ChannelName { get; set; }
}