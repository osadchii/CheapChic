using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.TelegramChannelTable)]
public class TelegramChannelEntity : BaseEntity
{
    [Required]
    public long ChatId { get; set; }
    
    [Required]
    [ForeignKey(nameof(TelegramUserEntity))]
    public Guid OwnerId { get; set; }

    public TelegramUserEntity Owner { get; set; }
}