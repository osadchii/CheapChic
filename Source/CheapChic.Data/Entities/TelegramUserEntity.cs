using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[ChapChicTable(DatabaseConstant.TelegramUserTable)]
public class TelegramUserEntity : BaseEntity
{
    [Required] public long ChatId { get; set; }

    public string Username { get; set; }

    public string Name { get; set; }

    public string Lastname { get; set; }

    [Required] [DefaultValue(false)] public bool Disabled { get; set; }
}