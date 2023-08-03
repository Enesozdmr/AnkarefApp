using System.ComponentModel.DataAnnotations;

namespace AnkarefApp.Data;

public class User
{
    public DateTime CreatedAt;
    public Guid Id { get; set; }

    /*[Required]
    public string Name { get; set; }*/


    [Required] public string? Email { get; set; }

    [Required] public string? Password { get; set; }
}