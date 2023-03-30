using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.TelegramBotTable)]
public class TelegramBotEntity : BaseEntity
{
    [Required]
    [MaxLength(DatabaseLimit.TelegramBotToken)]
    public string Token { get; set; }

    [Required] [DefaultValue(false)] public bool Disabled { get; set; }

    [Required]
    [ForeignKey(nameof(TelegramUserEntity))]
    public Guid OwnerId { get; set; }

    public TelegramUserEntity Owner { get; set; }

    [Required]
    [MaxLength(DatabaseLimit.TelegramBotName)]
    public string Name { get; set; }

    [Required] 
    [DefaultValue(24)]
    public int PublishEveryHours { get; set; }

    [Required] 
    [DefaultValue(28)]
    public int PublishForDays { get; set; }

    [MaxLength(DatabaseLimit.TelegramBotCurrency)]
    public string Currency { get; set; }
}