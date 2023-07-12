using System.ComponentModel.DataAnnotations;

namespace AnkarefApp.Data;

public class User
{
    public Guid Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Timestamp] public DateTime CreatedAt;

}