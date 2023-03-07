using System.ComponentModel.DataAnnotations;

namespace CheapChic.Data.Entities;

public abstract class BaseEntity
{
    [Key] public Guid Id { get; set; }

    [Required] public DateTime CreatedOn { get; set; }

    [Required] public DateTime ChangedOn { get; set; }
}