using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.AdPhotoTable)]
public class AdPhotoEntity : BaseEntity
{
    [Required]
    [ForeignKey(nameof(AdEntity))]
    public Guid AdId { get; set; }

    public AdEntity Ad { get; set; }

    [Required]
    [ForeignKey(nameof(PhotoEntity))]
    public Guid PhotoId { get; set; }

    public PhotoEntity Photo { get; set; }
}