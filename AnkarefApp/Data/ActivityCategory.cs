using System.ComponentModel.DataAnnotations;

namespace AnkarefApp.Data;

public class ActivityCategory
{
    public Guid Id { get; set; }

    [Required] public string? Name { get; set; }
}