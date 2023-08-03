using System.ComponentModel.DataAnnotations;

namespace AnkarefApp.Data;

public class ActivityTable
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    public Guid CreatingUserId { get; set; }
    public User? CreatingUser { get; set; }
    public Guid ActivityCategoryId { get; set; }
    public ActivityCategory? ActivityCategory { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; }
}