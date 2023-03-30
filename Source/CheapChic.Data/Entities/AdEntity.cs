using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.AdTable)]
public class AdEntity : BaseEntity
{
    [ForeignKey(nameof(TelegramBotEntity))]
    public Guid BotId { get; set; }

    public TelegramBotEntity Bot { get; set; }

    [ForeignKey(nameof(TelegramUserEntity))]
    public Guid UserId { get; set; }

    public TelegramUserEntity User { get; set; }

    [Required] 
    [MaxLength(DatabaseLimit.AdAction)]
    public string Action { get; set; }

    [Required]
    [MaxLength(DatabaseLimit.AdName)]
    public string Name { get; set; }

    [Required]
    [MaxLength(DatabaseLimit.AdDescription)]
    public string Description { get; set; }

    [Required] public DateTime Date { get; set; }

    public DateTime? DateOfLastPublication { get; set; }

    public bool Disable { get; set; }

    public decimal? Price { get; set; }
}