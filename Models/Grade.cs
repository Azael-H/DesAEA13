using System.ComponentModel.DataAnnotations;

namespace DesAEA13.Models;

public class Grade
{
    [Key]
    public int IdGrade { get; set; }

    [Required]
    [StringLength(45)]
    public string Name { get; set; } = string.Empty;

    [StringLength(45)]
    public string? Description { get; set; }

    public bool Active { get; set; } = true;

    public ICollection<Student> Students { get; set; } = new List<Student>();
}
