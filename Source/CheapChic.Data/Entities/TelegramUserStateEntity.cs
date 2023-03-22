using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;
using CheapChic.Data.Enums;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.TelegramUserStateTable)]
public class TelegramUserStateEntity : BaseEntity
{
    [ForeignKey(nameof(TelegramBotEntity))]
    public Guid? TelegramBotId { get; set; }

    public TelegramBotEntity TelegramBot { get; set; }

    [Required]
    [ForeignKey(nameof(TelegramUserEntity))]
    public Guid UserId { get; set; }

    public TelegramUserEntity User { get; set; }

    [Required] 
    public State State { get; set; }

    public string Data { get; set; }
}