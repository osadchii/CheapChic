using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[ChapChicTable(DatabaseConstant.TelegramBotTable)]
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
}