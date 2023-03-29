using System.ComponentModel.DataAnnotations;
using CheapChic.Data.Attributes;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Entities;

[CheapChicTable(DatabaseConstant.PhotoTable)]
public class PhotoEntity : BaseEntity
{
    [Required] 
    public byte[] Content { get; set; }

    [Required]
    public long FileSize { get; set; }

    [Required] 
    [MaxLength(DatabaseLimit.PhotoHash)]
    public string Hash { get; set; }
}